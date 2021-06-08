using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.PushNotification.Domain.Models.Enums;

namespace Service.PushNotification.Domain.Models
{
    [DataContract]
    public class NotificationTemplate
    {
        [DataMember(Order = 1)]
        public NotificationTypeEnum Type { get; set; }
        
        [DataMember(Order = 2)]
        public string DefaultBrand { get; set; }
        
        [DataMember(Order = 3)]
        public string DefaultLang { get; set; }
        
        [DataMember(Order = 4)]
        public Dictionary<(string brand,string lang),(string topic, string body)> Bodies { get; set; }
        
        [DataMember(Order = 5)]
        public List<string> Params { get; set; }

    }
}