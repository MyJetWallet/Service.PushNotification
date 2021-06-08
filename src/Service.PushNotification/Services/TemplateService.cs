using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Service.PushNotification.Domain.Extensions;
using Service.PushNotification.Domain.Models;
using Service.PushNotification.Domain.Models.Enums;
using Service.PushNotification.Domain.NoSql;
using Service.PushNotification.Grpc;
using Service.PushNotification.Grpc.Models.Requests;
using Service.PushNotification.Grpc.Models.Responses;

namespace Service.PushNotification.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ILogger<TemplateService> _logger;
        private readonly IMyNoSqlServerDataWriter<TemplateNoSqlEntity> _templateWriter;

        private const string _defaultBrand = "Default";
        private const string _defaultLang = "En";

        private readonly IDictionary<NotificationTypeEnum, (string,string)> _defaultLangTemplateBodies =
            new Dictionary<NotificationTypeEnum, (string,string)>
            {
                {NotificationTypeEnum.LoginNotification, ("Successful log in","Successful log in account from IP ${IP} (${DATE})")},
                {NotificationTypeEnum.TradeNotification, ("Trade made: ${SYMBOL}","Trade made: ${SYMBOL}, price ${PRICE}, volume ${VOLUME}")}
            };

        private readonly IDictionary<NotificationTypeEnum, List<string>> _templateBodyParams =
            new Dictionary<NotificationTypeEnum, List<string>>
            {
                {NotificationTypeEnum.LoginNotification, new List<string> {"${IP}", "${DATE}"}},
                {NotificationTypeEnum.TradeNotification, new List<string> {"${SYMBOL}", "${PRICE}", "${VOLUME}"}}
            };

        public TemplateService(IMyNoSqlServerDataWriter<TemplateNoSqlEntity> templateWriter,
            ILogger<TemplateService> logger)
        {
            _templateWriter = templateWriter;
            _logger = logger;
        }

        public async Task<(string,string)> GetMessageTemplate(NotificationTypeEnum type, string brand, string lang)
        {
            var partKey = TemplateNoSqlEntity.GeneratePartitionKey();
            var rowKey = TemplateNoSqlEntity.GenerateRowKey(type);
            var template = (await _templateWriter.GetAsync(partKey, rowKey)).ToTemplate();

            (string, string) body; 
            if (!template.Bodies.TryGetValue((brand, lang), out body))
            {
                _logger.LogWarning("No template found for {Type}, {Brand} and {Lang}", type, brand, lang);
                if (!template.Bodies.TryGetValue((template.DefaultBrand, lang), out body))
                {
                    _logger.LogWarning("No template found for  {Type}, {DefaultBrand} and {Lang}", type,
                        template.DefaultBrand, lang);
                    if (!template.Bodies.TryGetValue((template.DefaultBrand, template.DefaultLang), out body))
                    {
                        _logger.LogError("No default template for type {Type}", type);
                        throw new Exception();
                    }
                }
            }

            return body;
        }

        public async Task<TemplateListResponse> GetAllTemplates()
        {
            try
            {
                var templateEntities = (await _templateWriter.GetAsync())?.ToList();

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

            var template = (await _templateWriter.GetAsync(partKey, rowKey)).ToTemplate();
            template.Bodies[(request.Brand, request.Lang)] = (request.TemplateTopic, request.TemplateBody);

            await _templateWriter.InsertOrReplaceAsync(TemplateNoSqlEntity.Create(template));
        }

        private Dictionary<(string, string), (string, string)> GetDefaultTemplateBodies(NotificationTypeEnum type, string brand,
            string lang)
        {
            return new Dictionary<(string, string), (string, string)>
            {
                {
                    (brand, lang), _defaultLangTemplateBodies[type]
                }
            };
        }

        private List<string> GetTemplateBodyParams(NotificationTypeEnum type)
        {
            return _templateBodyParams[type];
        }
    }
}