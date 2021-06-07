using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models.Responses
{
    [DataContract]
    public class GetUserTokensResponse
    {
        [DataMember(Order = 1)] public string[] Tokens { get; set; }
    }
}