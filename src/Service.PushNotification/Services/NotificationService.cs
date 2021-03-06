using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Service.PushNotification.Domain.Models.Enums;
using Service.PushNotification.Grpc;
using Service.PushNotification.Grpc.Models.Requests;

namespace Service.PushNotification.Services
{
    public class NotificationService : INotificationService
    {
        private readonly TokenManager _tokenManager;
        private readonly ILogger<NotificationService> _logger;
        private readonly IFirebaseNotificationSender _firebaseSender;
        private readonly IHistoryRecordingService _historyService;
        private readonly TemplateService _templateService;

        public NotificationService(TokenManager tokenManager, ILogger<NotificationService> logger,
            IFirebaseNotificationSender firebaseSender, IHistoryRecordingService historyService,
            TemplateService templateService)
        {
            _tokenManager = tokenManager;
            _logger = logger;
            _firebaseSender = firebaseSender;
            _historyService = historyService;
            _templateService = templateService;
        }

        public async Task SendPushLogin(LoginPushRequest request)
        {
            _logger.LogInformation("Executing SendPushLogin for clientId {clientId}", request.ClientId);
            var tokens = await _tokenManager.GetUserTokens(new GetUserTokensRequest
            {
                ClientId = request.ClientId
            });
            var msgId = Guid.NewGuid();
            var parameters = new List<string>
            {
                request.Ip,
                request.Date.ToString(CultureInfo.InvariantCulture)
            };
            await _historyService.RecordNotification(msgId, NotificationTypeEnum.LoginNotification, request.ClientId,
                parameters);

            var langGroups = tokens.Tokens.GroupBy(t => t.UserLocale);
            foreach (var lang in langGroups)
            {
                var brand = lang.First().BrandId;
                var (templateTopic, templateBody) =
                    await _templateService.GetMessageTemplate(NotificationTypeEnum.LoginNotification, brand, lang.Key);

                var title = templateTopic    
                    .Replace("${IP}", request.Ip)
                    .Replace("${DATE}", request.Date.ToString(CultureInfo.InvariantCulture));
                
                var body = templateBody
                    .Replace("${IP}", request.Ip)
                    .Replace("${DATE}", request.Date.ToString(CultureInfo.InvariantCulture));

                var pushTokens = lang.Select(t => t).ToArray();
                await _firebaseSender.SendNotificationPush(msgId, pushTokens, title, body);
            }
        }

        public async Task SendPushTrade(TradePushRequest request)
        {            
            _logger.LogInformation("Executing SendPushTrade for clientId {clientId}", request.ClientId);
            var tokens = await _tokenManager.GetUserTokens(new GetUserTokensRequest
            {
                ClientId = request.ClientId
            });
            var msgId = Guid.NewGuid();
            var parameters = new List<string>
            {
                request.Amount.ToString(CultureInfo.InvariantCulture),
                request.Price.ToString(CultureInfo.InvariantCulture),
                request.Instrument
            };
            await _historyService.RecordNotification(msgId, NotificationTypeEnum.TradeNotification, request.ClientId,
                parameters);

            var langGroups = tokens.Tokens.GroupBy(t => t.UserLocale);
            foreach (var lang in langGroups)
            {
                var brand = lang.First().BrandId;
                var (templateTopic, templateBody) =
                    await _templateService.GetMessageTemplate(NotificationTypeEnum.TradeNotification, brand, lang.Key);

                var title = templateTopic    
                    .Replace("${SYMBOL}", request.Instrument)
                    .Replace("${PRICE}", request.Price.ToString(CultureInfo.InvariantCulture))
                    .Replace("${VOLUME}", request.Amount.ToString(CultureInfo.InvariantCulture));
                
                var body = templateBody
                    .Replace("${SYMBOL}", request.Instrument)
                    .Replace("${PRICE}", request.Price.ToString(CultureInfo.InvariantCulture))
                    .Replace("${VOLUME}", request.Amount.ToString(CultureInfo.InvariantCulture));

                var pushTokens = lang.Select(t => t).ToArray();
                await _firebaseSender.SendNotificationPush(msgId, pushTokens, title, body);
            }
        }

        public async Task SendPushCryptoDeposit(DepositRequest request)
        {
            await SendPush(NotificationTypeEnum.CryptoDepositReceive, request.ClientId, 
                s => s
                    .Replace("${SYMBOL}", request.Symbol)
                    .Replace("${AMOUNT}", request.Amount.ToString(CultureInfo.InvariantCulture)),
                request.Symbol, request.Amount.ToString(CultureInfo.InvariantCulture)
            );
        }

        public async Task SendPushCryptoWithdrawalStarted(CryptoWithdrawalRequest request)
        {
            _logger.LogInformation("Executing SendPushCryptoWithdrawalStarted for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.CryptoWithdrawalStarted, request.ClientId,
                    s => s
                        .Replace("${SYMBOL}", request.Symbol)
                        .Replace("${AMOUNT}", request.Amount.ToString(CultureInfo.InvariantCulture))
                        .Replace("${DESTINATION}", request.Destination),
                    request.Symbol, request.Amount.ToString(CultureInfo.InvariantCulture), request.Destination
                );
        }

        public async Task SendPushCryptoWithdrawalComplete(CryptoWithdrawalRequest request)
        {
            _logger.LogInformation("Executing SendPushCryptoWithdrawalComplete for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.CryptoWithdrawalComplete, request.ClientId,
                    s => s
                        .Replace("${SYMBOL}", request.Symbol)
                        .Replace("${AMOUNT}", request.Amount.ToString(CultureInfo.InvariantCulture))
                        .Replace("${DESTINATION}", request.Destination),
                    request.Symbol, request.Amount.ToString(CultureInfo.InvariantCulture), request.Destination
                );
        }

        public async Task SendPushCryptoWithdrawalDecline(CryptoWithdrawalRequest request)
        {
            _logger.LogInformation("Executing SendPushCryptoWithdrawalDecline for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.CryptoWithdrawalDecline, request.ClientId,
                    s => s
                        .Replace("${SYMBOL}", request.Symbol)
                        .Replace("${AMOUNT}", request.Amount.ToString(CultureInfo.InvariantCulture))
                        .Replace("${DESTINATION}", request.Destination),
                    request.Symbol, request.Amount.ToString(CultureInfo.InvariantCulture), request.Destination
                );
        }

        public async Task SendPushCryptoWithdrawalCancel(CryptoWithdrawalRequest request)
        {
            _logger.LogInformation("Executing SendPushCryptoWithdrawalCancel for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.CryptoWithdrawalCancel, request.ClientId,
                s => s
                    .Replace("${SYMBOL}", request.Symbol)
                    .Replace("${AMOUNT}", request.Amount.ToString(CultureInfo.InvariantCulture)),
                request.Symbol, request.Amount.ToString(CultureInfo.InvariantCulture)
            );        
        }

        public async Task SendPushCryptoConvert(ConvertRequest request)
        {
            _logger.LogInformation("Executing SendPushCryptoConvert for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.Swap, request.ClientId,
                s => s
                    .Replace("${FROM_ASSET}", request.FromSymbol)
                    .Replace("${FROM_AMOUNT}", request.FromAmount.ToString(CultureInfo.InvariantCulture))
                    .Replace("${TO_ASSET}", request.ToSymbol)
                    .Replace("${TO_AMOUNT}", request.ToAmount.ToString(CultureInfo.InvariantCulture)),
                request.FromSymbol, request.FromAmount.ToString(CultureInfo.InvariantCulture), request.ToSymbol, request.ToAmount.ToString(CultureInfo.InvariantCulture)
            );
        }

        public async Task SendPushTransferSend(SendPushTransferRequest request)
        {
            _logger.LogInformation("Executing SendPushTransferSend for clientId {clientId}; Amount: {amount}; Asset: {symbol}", 
                request.SenderClientId, request.Amount, request.AssetSymbol);
            
            await SendPush(NotificationTypeEnum.SendTransfer, request.SenderClientId, 
                s => s
                    .Replace("${AMOUNT}", request.Amount.ToString(CultureInfo.InvariantCulture))
                    .Replace("${ASSET_SYMBOL}", request.AssetSymbol)
                    .Replace("${DESTINATION_PHONE_NUMBER}", request.DestinationPhoneNumber),
                request.Amount.ToString(CultureInfo.InvariantCulture), request.AssetSymbol
            );
        }

        public async Task SendPushTransferReceive(SendPushTransferRequest request)
        {
            _logger.LogInformation("Executing SendPushTransferReceive for clientId {clientId}; Amount: {amount}; Asset: {symbol}", 
                request.DestinationClientId, request.Amount, request.AssetSymbol);
            
            await SendPush(NotificationTypeEnum.ReceiveTransfer, request.DestinationClientId, 
                    s => s
                        .Replace("${AMOUNT}", request.Amount.ToString(CultureInfo.InvariantCulture))
                        .Replace("${ASSET_SYMBOL}", request.AssetSymbol)
                        .Replace("${SENDER_PHONE_NUMBER}", request.SenderPhoneNumber),
                    request.Amount.ToString(CultureInfo.InvariantCulture), request.AssetSymbol
                );
        }

        public async Task SendPushKycDocumentsDeclined(KycNotificationRequest request)
        {
            _logger.LogInformation("Executing SendPushKycDocumentsDeclined for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.KycDocumentsDeclined, request.ClientId,
                s => s);     
        }

        public async Task SendPushKycDocumentsApproved(KycNotificationRequest request)
        {
            _logger.LogInformation("Executing SendPushKycDocumentsApproved for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.KycDocumentsApproved, request.ClientId,
                s => s);
        }

        public async Task SendPushKycUserBanned(KycNotificationRequest request)
        {
            _logger.LogInformation("Executing SendPushKycUserBanned for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.KycBanned, request.ClientId,
                s => s);
        }

        public async Task SendTwoFaEnabled(TwoFaRequest request)
        {
            _logger.LogInformation("Executing SendTwoFaEnabled for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.TwoFaEnabled, request.ClientId,
                s => s);
        }
        
        public async Task SendTwoFaDisabled(TwoFaRequest request)
        {
            _logger.LogInformation("Executing SendTwoFaDisabled for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.TwoFaDisabled, request.ClientId,
                s => s);
        }

        public async Task SendAutoInvestCreate_Daily(AutoInvestCreateRequest request)
        {
            _logger.LogInformation("Executing SendAutoInvestCreate_Daily for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.AutoInvestCreateDaily, request.ClientId,
                s => s
                    .Replace("${FROM_ASSET}", request.FromAsset)
                    .Replace("${FROM_AMOUNT}", request.FromAmount.ToString(CultureInfo.InvariantCulture))
                    .Replace("${TO_ASSET}", request.ToAsset),
                request.FromAsset, request.FromAmount.ToString(CultureInfo.InvariantCulture), request.ToAsset);
        }

        public async Task SendAutoInvestCreate_Weekly(AutoInvestCreateRequest request)
        {
            _logger.LogInformation("Executing SendAutoInvestCreate_Weekly for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.AutoInvestCreateWeekly, request.ClientId,
                s => s
                    .Replace("${FROM_ASSET}", request.FromAsset)
                    .Replace("${FROM_AMOUNT}", request.FromAmount.ToString(CultureInfo.InvariantCulture))
                    .Replace("${TO_ASSET}", request.ToAsset),
                request.FromAsset, request.FromAmount.ToString(CultureInfo.InvariantCulture), request.ToAsset);
        }

        public async Task SendAutoInvestCreate_BiWeekly(AutoInvestCreateRequest request)
        {
            _logger.LogInformation("Executing SendAutoInvestCreate_BiWeekly for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.AutoInvestCreateBiWeekly, request.ClientId,
                s => s
                    .Replace("${FROM_ASSET}", request.FromAsset)
                    .Replace("${FROM_AMOUNT}", request.FromAmount.ToString(CultureInfo.InvariantCulture))
                    .Replace("${TO_ASSET}", request.ToAsset),
                request.FromAsset, request.FromAmount.ToString(CultureInfo.InvariantCulture), request.ToAsset);
        }

        public async Task SendAutoInvestCreate_Monthly(AutoInvestCreateRequest request)
        {
            _logger.LogInformation("Executing SendAutoInvestCreate_Monthly for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.AutoInvestCreateMonthly, request.ClientId,
                s => s
                    .Replace("${FROM_ASSET}", request.FromAsset)
                    .Replace("${FROM_AMOUNT}", request.FromAmount.ToString(CultureInfo.InvariantCulture))
                    .Replace("${TO_ASSET}", request.ToAsset),
                request.FromAsset, request.FromAmount.ToString(CultureInfo.InvariantCulture), request.ToAsset);
        }
        
        public async Task SendAutoInvestExecute(AutoInvestExecuteRequest request)
        {
            _logger.LogInformation("Executing SendAutoInvestExecute for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.AutoInvestExecute, request.ClientId,
                s => s
                    .Replace("${FROM_ASSET}", request.FromAsset)
                    .Replace("${FROM_AMOUNT}", request.FromAmount.ToString(CultureInfo.InvariantCulture))
                    .Replace("${TO_ASSET}", request.ToAsset)
                    .Replace("${TO_AMOUNT}", request.ToAmount.ToString(CultureInfo.InvariantCulture))
                    .Replace("${EXECUTION_TIME}", request.ExecutionTime.ToString("R")),
                request.FromAsset, request.FromAmount.ToString(CultureInfo.InvariantCulture), request.ToAsset, request.ToAmount.ToString(CultureInfo.InvariantCulture), request.ExecutionTime.ToString("s")
            );        
        }

        public async Task SendAutoInvestFail_InvalidPair(AutoInvestFailRequest request)
        {
            _logger.LogInformation("Executing SendAutoInvestFail_InvalidPair for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.AutoInvestFailInvalidPair, request.ClientId,
                s => s
                    .Replace("${FROM_ASSET}", request.FromAsset)
                    .Replace("${FROM_AMOUNT}", request.FromAmount.ToString(CultureInfo.InvariantCulture))
                    .Replace("${TO_ASSET}", request.ToAsset)
                    .Replace("${FAIL_TIME}", request.FailTime.ToString("R")),
                request.FromAsset, request.FromAmount.ToString(CultureInfo.InvariantCulture), request.ToAsset, request.FailTime.ToString("s")
            );
            
        }

        public async Task SendAutoInvestFail_LowBalance(AutoInvestFailRequest request)
        {
            _logger.LogInformation("Executing SendAutoInvestFail_LowBalance for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.AutoInvestFailLowBalance, request.ClientId,
                s => s
                    .Replace("${FROM_ASSET}", request.FromAsset)
                    .Replace("${FROM_AMOUNT}", request.FromAmount.ToString(CultureInfo.InvariantCulture))
                    .Replace("${TO_ASSET}", request.ToAsset)
                    .Replace("${FAIL_TIME}", request.FailTime.ToString("R")),
                request.FromAsset, request.FromAmount.ToString(CultureInfo.InvariantCulture), request.ToAsset, request.FailTime.ToString("s")
            );
            
        }

        public async Task SendAutoInvestFail_InternalError(AutoInvestFailRequest request)
        {
            _logger.LogInformation("Executing SendAutoInvestFail_InternalError for clientId {clientId}", request.ClientId);
            await SendPush(NotificationTypeEnum.AutoInvestFailInternalError, request.ClientId,
                s => s
                    .Replace("${FROM_ASSET}", request.FromAsset)
                    .Replace("${FROM_AMOUNT}", request.FromAmount.ToString(CultureInfo.InvariantCulture))
                    .Replace("${TO_ASSET}", request.ToAsset)
                    .Replace("${FAIL_TIME}", request.FailTime.ToString("R")),
                request.FromAsset, request.FromAmount.ToString(CultureInfo.InvariantCulture), request.ToAsset, request.FailTime.ToString("s")
            );
        }
        

        private async Task SendPush(NotificationTypeEnum type, string clientId, Func<string, string> applyParams, params string[] paramToHistory)
        {
            var tokens = await _tokenManager.GetUserTokens(new GetUserTokensRequest
            {
                ClientId = clientId
            });
            var msgId = Guid.NewGuid();
            var parameters = new List<string>(paramToHistory);
            await _historyService.RecordNotification(msgId, type, clientId, parameters);

            var langGroups = tokens.Tokens.GroupBy(t => t.UserLocale);
            foreach (var lang in langGroups)
            {
                var brand = lang.First().BrandId;
                var (templateTopic, templateBody) =
                    await _templateService.GetMessageTemplate(type, brand, lang.Key);

                var title = applyParams.Invoke(templateTopic);

                var body = applyParams.Invoke(templateBody);

                var pushTokens = lang.Select(t => t).ToArray();
                await _firebaseSender.SendNotificationPush(msgId, pushTokens, title, body);
            }
        }
    }
}