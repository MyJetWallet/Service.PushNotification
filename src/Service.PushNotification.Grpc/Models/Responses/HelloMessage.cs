using System.Runtime.Serialization;
using Service.PushNotification.Domain.Models;

namespace Service.PushNotification.Grpc.Models.Responses
{
    [DataContract]
    public class HelloMessage : IHelloMessage
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }
    }
}