using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models.Requests
{
    [DataContract]
    public class HelloRequest
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }
    }
}