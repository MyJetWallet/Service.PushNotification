using System.ServiceModel;
using System.Threading.Tasks;
using Service.PushNotification.Grpc.Models;

namespace Service.PushNotification.Grpc
{
    [ServiceContract]
    public interface ITokenManager
    {
        [OperationContract]
        Task RegisterToken(RegisterTokenRequest request);
        
        //move to non-grpc service?
        [OperationContract]
        Task<GetUserTokensResponse> GetUserTokens(GetUserTokensRequest request);
    }
}