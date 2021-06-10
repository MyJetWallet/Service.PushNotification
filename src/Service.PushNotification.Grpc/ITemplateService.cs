using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Service.PushNotification.Domain.Models.Enums;
using Service.PushNotification.Grpc.Models.Requests;
using Service.PushNotification.Grpc.Models.Responses;

namespace Service.PushNotification.Grpc
{
    [ServiceContract]
    public interface ITemplateService
    {
        [OperationContract]Task<TemplateListResponse> GetAllTemplates();

        [OperationContract]Task EditTemplate(TemplateEditRequest request);

        [OperationContract]Task DeleteBody(TemplateEditRequest request);

        Task<(string, string)> GetMessageTemplate(NotificationTypeEnum type, string brand, string lang);
        Task CreateDefaultTemplates();

    }
}