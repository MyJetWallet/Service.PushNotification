using System.ServiceModel;
using System.Threading.Tasks;
using Service.PushNotification.Grpc.Models.Requests;
using Service.PushNotification.Grpc.Models.Responses;

namespace Service.PushNotification.Grpc
{
    [ServiceContract]
    public interface ITemplateService
    {
        [OperationContract]Task<TemplateListResponse> GetAllTemplates();

        [OperationContract]Task EditTemplate(TemplateEditRequest request);

    }
}