using System.ServiceModel;
using System.Threading.Tasks;
using Service.PushNotification.Grpc.Models;
using Service.PushNotification.Grpc.Models.Requests;

namespace Service.PushNotification.Grpc
{
    [ServiceContract]
    public interface INotificationService
    {
        [OperationContract]
        Task SendPushLogin(LoginPushRequest request);
        
        [OperationContract]
        Task SendPushTrade(TradePushRequest request);
    }
}