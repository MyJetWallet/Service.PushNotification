using System.ServiceModel;
using System.Threading.Tasks;
using Service.PushNotification.Grpc.Models.Requests;

namespace Service.PushNotification.Grpc
{
    [ServiceContract]
    public interface INotificationService
    {
        [OperationContract]
        Task SendPushLogin(LoginPushRequest request);
        
        [OperationContract]
        Task SendPushTrade(TradePushRequest request);

        [OperationContract]
        Task SendPushCryptoDeposit(DepositRequest request);

        [OperationContract]
        Task SendPushCryptoWithdrawalStarted(CryptoWithdrawalRequest request);

        [OperationContract]
        Task SendPushCryptoWithdrawalComplete(CryptoWithdrawalRequest request);

        [OperationContract]
        Task SendPushCryptoWithdrawalDecline(CryptoWithdrawalRequest request);
        
        [OperationContract]
        Task SendPushCryptoWithdrawalCancel(CryptoWithdrawalRequest request);

        [OperationContract]
        Task SendPushCryptoConvert(ConvertRequest request);
        
        [OperationContract]
        Task SendPushTransferSend(SendPushTransferRequest request);
        
        [OperationContract]
        Task SendPushTransferReceive(SendPushTransferRequest request);
        
        [OperationContract]
        Task SendPushKycDocumentsDeclined(KycNotificationRequest request);
        
        [OperationContract]
        Task SendPushKycDocumentsApproved(KycNotificationRequest request);
        
        [OperationContract]
        Task SendPushKycUserBanned(KycNotificationRequest request);
    }
}