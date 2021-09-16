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
    public class TemplateServiceGrpc : ITemplateService
    {
        private readonly TemplateService _templateService;

        public TemplateServiceGrpc(TemplateService templateService)
        {
            _templateService = templateService;
        }

        public async Task DeleteBody(TemplateEditRequest request) => await _templateService.DeleteBody(request);

        public async Task EditDefaultValues(TemplateEditRequest request) =>
            await _templateService.EditDefaultValues(request);

        public async Task<(string, string)> GetMessageTemplate(NotificationTypeEnum type, string brand, string lang) =>
            await _templateService.GetMessageTemplate(type, brand, lang);

        public async Task<TemplateListResponse> GetAllTemplates() => await _templateService.GetAllTemplates();

        public async Task EditTemplate(TemplateEditRequest request) => await _templateService.EditTemplate(request);
    }
}