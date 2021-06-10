using System.Runtime.Serialization;
using Service.PushNotification.Domain.Models.Enums;

namespace Service.PushNotification.Grpc.Models.Requests
{
    [DataContract]
    public class TemplateEditRequest
    {
        [DataMember(Order = 1)] public NotificationTypeEnum Type { get; set; }
        
        [DataMember(Order = 2)] public string Brand { get; set; }

        [DataMember(Order = 3)] public string Lang { get; set; }

        [DataMember(Order = 4)] public string TemplateTopic { get; set; }
        
        [DataMember(Order = 5)] public string TemplateBody { get; set; }
        
        [DataMember(Order = 6)] public string DefaultBrand { get; set; }

        [DataMember(Order = 7)] public string DefaultLang { get; set; }
    }
}