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
    public class TemplateService
    {
        private readonly ILogger<TemplateService> _logger;
        private readonly IMyNoSqlServerDataWriter<TemplateNoSqlEntity> _templateWriter;

        private const string _defaultBrand = "default";
        private const string _defaultLang = "en";
        
        public TemplateService(IMyNoSqlServerDataWriter<TemplateNoSqlEntity> templateWriter,
            ILogger<TemplateService> logger)
        {
            _templateWriter = templateWriter;
            _logger = logger;
        }

        public async Task DeleteBody(TemplateEditRequest request)
        {
            try
            {
                var partKey = TemplateNoSqlEntity.GeneratePartitionKey();
                var rowKey = TemplateNoSqlEntity.GenerateRowKey(request.Type);
                var template = (await _templateWriter.GetAsync(partKey, rowKey)).ToTemplate();
                if (template.DefaultBrand == request.Brand && template.DefaultLang == request.Lang)
                {
                    _logger.LogWarning("Unable to delete default template for type {Type}", template.Type);
                    throw new InvalidOperationException($"Unable to delete default template for type {template.Type}");
                }

                if (template.Bodies.ContainsKey((request.Brand, request.Lang)))
                {
                    template.Bodies.Remove((request.Brand, request.Lang));
                    await _templateWriter.InsertOrReplaceAsync(TemplateNoSqlEntity.Create(template));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "When trying to delete body for template {Type} with brand {Brand} and lang {Lang}", request.Type, request.Brand, request.Lang);
                throw;
            }
        }

        public async Task<(string,string)> GetMessageTemplate(NotificationTypeEnum type, string brand, string lang)
        {
            var partKey = TemplateNoSqlEntity.GeneratePartitionKey();
            var rowKey = TemplateNoSqlEntity.GenerateRowKey(type);
            var template = (await _templateWriter.GetAsync(partKey, rowKey)).ToTemplate();

            brand = brand.ToLower();
            lang = lang.ToLower();
            
            (string, string) body; 
            if (!template.Bodies.TryGetValue((brand, lang), out body))
            {
                _logger.LogWarning("No template found for {Type}, {Brand} and {Lang}", type, brand, lang);
                if(!template.Bodies.TryGetValue((brand, template.DefaultLang), out body))
                {
                    _logger.LogWarning("No template found for  {Type}, {Brand} and {DefaultLang}", type,
                        brand, template.DefaultLang);
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
            }

            return body;
        }

        public async Task CreateDefaultTemplates()
        {
            var templateEntities = (await _templateWriter.GetAsync())?.ToList();
            foreach (var type in Enum.GetValues(typeof(NotificationTypeEnum)).Cast<NotificationTypeEnum>())
            {
                var templateEntity = templateEntities?.FirstOrDefault(e => e.Type == type);
                if (templateEntity == null)
                {
                    var template = new NotificationTemplate
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
            }
        }
        public async Task<TemplateListResponse> GetAllTemplates()
        {
            try
            {
                var templateEntities = (await _templateWriter.GetAsync())?.ToList();
                var templates = new List<NotificationTemplate>();
                foreach (var type in Enum.GetValues(typeof(NotificationTypeEnum)).Cast<NotificationTypeEnum>())
                {
                    var templateEntity = templateEntities?.FirstOrDefault(e => e.Type == type);

                    if (templateEntity != null)
                    {
                        templates.Add(templateEntity.ToTemplate());
                    }
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
            template.Bodies[(request.Brand.ToLower(), request.Lang.ToLower())] = (request.TemplateTopic, request.TemplateBody);

            await _templateWriter.InsertOrReplaceAsync(TemplateNoSqlEntity.Create(template));
        }
        
        public async Task EditDefaultValues(TemplateEditRequest request)
        {
            var partKey = TemplateNoSqlEntity.GeneratePartitionKey();
            var rowKey = TemplateNoSqlEntity.GenerateRowKey(request.Type);
            request.Brand = request.Brand.ToLower();
            request.Lang = request.Lang.ToLower();

            var template = (await _templateWriter.GetAsync(partKey, rowKey)).ToTemplate();
            template.DefaultBrand = request.Brand;
            template.DefaultLang = request.Lang;

            if (string.IsNullOrWhiteSpace(request.TemplateBody) && string.IsNullOrWhiteSpace(request.TemplateTopic))
            {
                template.Bodies[(request.Brand, request.Lang)] =
                    NotificationTypeDefaults.DefaultLangTemplateBodies[template.Type];
            }
            else
            {
                template.Bodies[(request.Brand, request.Lang)] =
                    (request.TemplateTopic, request.TemplateBody);
            }
            await _templateWriter.InsertOrReplaceAsync(TemplateNoSqlEntity.Create(template));
        }

        private Dictionary<(string, string), (string, string)> GetDefaultTemplateBodies(NotificationTypeEnum type, string brand,
            string lang)
        {
            try
            {
                return new Dictionary<(string, string), (string, string)>
                {
                    {
                        (brand, lang), NotificationTypeDefaults.DefaultLangTemplateBodies[type]
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "No default template body found for type {Type}", type);
                throw;
            }
        }

        private List<string> GetTemplateBodyParams(NotificationTypeEnum type)
        {
            try
            {
                return NotificationTypeDefaults.TemplateBodyParams[type];
            }
            catch (Exception e)
            {
                _logger.LogError(e, "No default template params found for type {Type}", type);
                throw;
            }
        }
    }
}