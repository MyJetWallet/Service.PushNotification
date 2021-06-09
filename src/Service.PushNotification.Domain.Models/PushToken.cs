using System;
using System.Runtime.Serialization;

namespace Service.PushNotification.Domain.Models
{
    [DataContract]
    public class PushToken
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }
        
        [DataMember(Order = 2)]public string RootSessionId { get; set; }
        
        [DataMember(Order = 3)]public string Token { get; set; }
        
        [DataMember(Order = 4)]public string UserLocale { get; set; }
        
        [DataMember(Order = 5)]public string BrandId { get; set; }
        
        [DataMember(Order = 6)]public DateTime Registered { get; set; }
    }
}