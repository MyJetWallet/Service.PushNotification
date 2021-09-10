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
        CryptoDepositReceive,
        Swap
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
            };
    }
}