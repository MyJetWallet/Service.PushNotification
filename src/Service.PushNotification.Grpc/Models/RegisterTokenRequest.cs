using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models
{
    [DataContract]
    public class RegisterTokenRequest
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }
        
        [DataMember(Order = 2)]public string RootSessionId { get; set; }
        
        [DataMember(Order = 3)]public string Token { get; set; }
        
        [DataMember(Order = 4)]public string UserLocale { get; set; }
    }
}