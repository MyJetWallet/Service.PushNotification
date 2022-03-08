using System.Collections.Generic;

namespace Service.PushNotification.Domain.Models.Enums
{
    public enum NotificationTypeEnum
    {
        LoginNotification,
        TradeNotification,
        CryptoWithdrawalStarted,
        CryptoWithdrawalComplete,
        CryptoWithdrawalDecline,
        CryptoWithdrawalCancel,
        CryptoDepositReceive,
        Swap,
        SendTransfer,
        ReceiveTransfer,
        KycDocumentsApproved,
        KycDocumentsDeclined,
        KycBanned,
    }

    public static class NotificationTypeDefaults
    {
        public static readonly IDictionary<NotificationTypeEnum, (string, string)> DefaultLangTemplateBodies =
            new Dictionary<NotificationTypeEnum, (string, string)>
            {
                {NotificationTypeEnum.LoginNotification, ("Successful log in","Successful log in account from IP ${IP} (${DATE})")},
                {NotificationTypeEnum.TradeNotification, ("Trade made: ${SYMBOL}","Trade made: ${SYMBOL}, price ${PRICE}, volume ${VOLUME}")},

                {NotificationTypeEnum.CryptoWithdrawalStarted, ("Withdrawal Started ${AMOUNT} ${SYMBOL}", "Started withdrawal of ${AMOUNT} ${SYMBOL} to address ${DESTINATION}")},
                {NotificationTypeEnum.CryptoWithdrawalComplete, ("Withdrawal Completed ${AMOUNT} ${SYMBOL}", "Completed withdrawal of ${AMOUNT} ${SYMBOL} to address ${DESTINATION}")},
                {NotificationTypeEnum.CryptoWithdrawalDecline, ("Withdrawal Decline ${AMOUNT} ${SYMBOL}", "Decline withdrawal of ${AMOUNT} ${SYMBOL} to address ${DESTINATION}")},
                {NotificationTypeEnum.CryptoDepositReceive, ("Deposit ${AMOUNT} ${SYMBOL}","Receive deposit of ${AMOUNT} ${SYMBOL}")},
                {NotificationTypeEnum.Swap, ("The trade was done","You have successfully trade ${FROM_ASSET} ${FROM_AMOUNT} for ${TO_AMOUNT} ${TO_ASSET}")},
                {NotificationTypeEnum.SendTransfer, ("The transfer was send","You successfully send ${AMOUNT} ${ASSET_SYMBOL} to ${DESTINATION_PHONE_NUMBER}")},
                {NotificationTypeEnum.ReceiveTransfer, ("The transfer was received","You received ${AMOUNT} ${ASSET_SYMBOL} from ${SENDER_PHONE_NUMBER}")},
                
                {NotificationTypeEnum.CryptoWithdrawalCancel, ("Withdrawal canceled ${AMOUNT} ${SYMBOL}","Withdrawal of ${AMOUNT} ${SYMBOL} has been canceled")},
                {NotificationTypeEnum.KycDocumentsApproved, ("Documents have been verified", "Documents have been verified.")},
                {NotificationTypeEnum.KycDocumentsDeclined, ("Documents have not been verified.", "Your documents have not been verified. Please upload documents again")},
                {NotificationTypeEnum.KycBanned, ("We are forced to refuse your service", "We are forced to refuse your service due to the requirements of the regulator")},

            };

        public static readonly IDictionary<NotificationTypeEnum, List<string>> TemplateBodyParams =
            new Dictionary<NotificationTypeEnum, List<string>>
            {
                {NotificationTypeEnum.LoginNotification, new List<string> {"${IP}", "${DATE}"}},
                {NotificationTypeEnum.TradeNotification, new List<string> {"${SYMBOL}", "${PRICE}", "${VOLUME}"}},
                {NotificationTypeEnum.CryptoWithdrawalStarted, new List<string>  {"${SYMBOL}", "${AMOUNT}", "${DESTINATION}"}},
                {NotificationTypeEnum.CryptoWithdrawalComplete, new List<string> {"${SYMBOL}", "${AMOUNT}", "${DESTINATION}"}},
                {NotificationTypeEnum.CryptoWithdrawalDecline, new List<string> {"${SYMBOL}", "${AMOUNT}", "${DESTINATION}"}},
                {NotificationTypeEnum.CryptoDepositReceive, new List<string> {"${SYMBOL}", "${AMOUNT}"}},
                {NotificationTypeEnum.Swap, new List<string> {"${FROM_ASSET}", "${FROM_AMOUNT}", "${TO_ASSET}", "${TO_AMOUNT}"}},
                {NotificationTypeEnum.SendTransfer, new List<string> {"${AMOUNT}", "${ASSET_SYMBOL}", "${DESTINATION_PHONE_NUMBER}"}},
                {NotificationTypeEnum.ReceiveTransfer, new List<string> {"${AMOUNT}", "${ASSET_SYMBOL}", "${SENDER_PHONE_NUMBER}"}},
                
                {NotificationTypeEnum.CryptoWithdrawalCancel,  new List<string>{"${AMOUNT}","${SYMBOL}"}},
                {NotificationTypeEnum.KycDocumentsApproved, new List<string>()},
                {NotificationTypeEnum.KycDocumentsDeclined, new List<string>()}, 
                {NotificationTypeEnum.KycBanned, new List<string>()},
            };
    }
}