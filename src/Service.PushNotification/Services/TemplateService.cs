using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Newtonsoft.Json;
using Service.PushNotification.Domain.Extensions;
using Service.PushNotification.Domain.Models;
using Service.PushNotification.Domain.Models.Enums;
using Service.PushNotification.Domain.NoSql;
using Service.PushNotification.Grpc;
using Service.PushNotification.Grpc.Models.Requests;
using Service.PushNotification.Grpc.Models.Responses;
using Enum = System.Enum;

namespace Service.PushNotification.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ILogger<TemplateService> _logger; 
        private readonly IMyNoSqlServerDataWriter<TemplateNoSqlEntity> _templateWriter;

        private const string _defaultBrand = "Default";
        private const string _defaultLang = "En";
        
        private readonly IDictionary<NotificationTypeEnum, string> _defaultLangTemplateBodies = new Dictionary<NotificationTypeEnum, string>
        {
            { NotificationTypeEnum.LoginNotification, "Successful log in account from IP ${IP} (${DATE})" },
            { NotificationTypeEnum.TradeNotification, "Trade made: ${SYMBOL}, price ${PRICE}, volume ${VOLUME}" }
        };
        private readonly IDictionary<NotificationTypeEnum, List<string>> _templateBodyParams = new Dictionary<NotificationTypeEnum, List<string>>
        {
            { NotificationTypeEnum.LoginNotification, new List<string> { "${IP}", "${DATE}" } },
            { NotificationTypeEnum.TradeNotification, new List<string> { "${SYMBOL}", "${PRICE}", "${VOLUME}" } }
        };

        public TemplateService(IMyNoSqlServerDataWriter<TemplateNoSqlEntity> templateWriter, ILogger<TemplateService> logger)
        {
            _templateWriter = templateWriter;
            _logger = logger;
        }


        public async Task<TemplateListResponse> GetAllTemplates()
        {
            try
            {
                var partKey = TemplateNoSqlEntity.GeneratePartitionKey();

                var templateEntities = (await _templateWriter.GetAsync(partKey))?.ToList(); //TODO: clean db and remove partkey

                var templates = new List<NotificationTemplate>();

                foreach (var type in Enum.GetValues(typeof(NotificationTypeEnum)).Cast<NotificationTypeEnum>())
                {
                    NotificationTemplate template;
                    var templateEntity = templateEntities?.FirstOrDefault(e => e.Type == type);
                    if (templateEntity == null)
                    {
                        template = new NotificationTemplate
                        {
                            Type = type,
                            DefaultBrand = _defaultBrand,
                            DefaultLang = _defaultLang,
                            Params = GetTemplateBodyParams(type),
                            Bodies = GetDefaultTemplateBodies(type, _defaultBrand, _defaultLang)
                        };

                        var newTemplateEntity = TemplateNoSqlEntity.Create(template);
                        await _templateWriter.InsertAsync(newTemplateEntity);

                        _logger.LogInformation("Template (ID: {templateId}) doesn't exist, creating the new one.",
                            type);
                    }
                    else
                    {
                        template = templateEntity.ToTemplate();
                    }

                    templates.Add(template);
                }

                return new TemplateListResponse
                {
                    Templates = templates
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "When getting all templates");
                throw;
            }
        }

        public async Task EditTemplate(TemplateEditRequest request)
        {
            var partKey = TemplateNoSqlEntity.GeneratePartitionKey();
            var rowKey = TemplateNoSqlEntity.GenerateRowKey(request.Type);

            var template = (await _templateWriter.GetAsync(partKey,rowKey)).ToTemplate();
            template.Bodies[(request.Brand, request.Lang)] = request.TemplateBody;
            
            await _templateWriter.InsertOrReplaceAsync(TemplateNoSqlEntity.Create(template));
        }

        private Dictionary<(string, string), string> GetDefaultTemplateBodies(NotificationTypeEnum type, string brand,
            string lang) =>
            new Dictionary<(string, string), string>()
            {
                {
                    (brand, lang), _defaultLangTemplateBodies[type]
                }
            };

        private List<string> GetTemplateBodyParams(NotificationTypeEnum type) => _templateBodyParams[type];
    }
}