using System.ServiceModel;
using System.Threading.Tasks;
using Service.PushNotification.Grpc.Models;

namespace Service.PushNotification.Grpc
{
    [ServiceContract]
    public interface IHelloService
    {
        [OperationContract]
        Task<HelloMessage> SayHelloAsync(HelloRequest request);
    }
}