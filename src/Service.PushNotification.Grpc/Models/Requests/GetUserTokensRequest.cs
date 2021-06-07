using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models.Requests
{
    [DataContract]
    public class GetUserTokensRequest
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }
    }
}