using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models.Requests
{
    [DataContract]
    public class SendPushTransferSendRequest
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }
        [DataMember(Order = 2)] public double Amount { get; set; }
        [DataMember(Order = 3)] public string AssetSymbol { get; set; }
        [DataMember(Order = 4)] public string SenderPhoneNumber { get; set; }
        [DataMember(Order = 5)] public string DestinationPhoneNumber { get; set; }
    }
}