using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models.Requests
{
    [DataContract]
    public class SendPushTransferRequest
    {
        [DataMember(Order = 1)] public string SenderClientId { get; set; }
        [DataMember(Order = 2)] public string DestinationClientId { get; set; }
        [DataMember(Order = 3)] public double Amount { get; set; }
        [DataMember(Order = 4)] public string AssetSymbol { get; set; }
        [DataMember(Order = 5)] public string SenderPhoneNumber { get; set; }
        [DataMember(Order = 6)] public string DestinationPhoneNumber { get; set; }
    }
}