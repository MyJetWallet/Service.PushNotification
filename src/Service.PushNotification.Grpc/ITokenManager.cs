using System.ServiceModel;
using System.Threading.Tasks;
using Service.PushNotification.Domain.Models;
using Service.PushNotification.Grpc.Models;
using Service.PushNotification.Grpc.Models.Requests;
using Service.PushNotification.Grpc.Models.Responses;

namespace Service.PushNotification.Grpc
{
    [ServiceContract]
    public interface ITokenManager
    {
        [OperationContract]
        Task RegisterToken(PushToken request);
        
        [OperationContract]
        Task<GetUserTokensResponse> GetUserTokens(GetUserTokensRequest request);
        
        [OperationContract]
        Task<GetAllTokensResponse> GetAllTokens();
    }
}