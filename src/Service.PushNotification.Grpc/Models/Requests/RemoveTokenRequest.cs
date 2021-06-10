using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models.Requests
{
    [DataContract]
    public class RemoveTokenRequest
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }

        [DataMember(Order = 2)] public string RootSessionId { get; set; }

    }
}