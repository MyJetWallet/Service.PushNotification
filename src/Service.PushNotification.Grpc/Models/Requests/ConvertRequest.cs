using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models.Requests
{
    [DataContract]
    public class ConvertRequest
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }

        [DataMember(Order = 2)] public string FromSymbol { get; set; }

        [DataMember(Order = 3)] public decimal FromAmount { get; set; }

        [DataMember(Order = 4)] public string ToSymbol { get; set; }

        [DataMember(Order = 5)] public decimal ToAmount { get; set; }
    }
}