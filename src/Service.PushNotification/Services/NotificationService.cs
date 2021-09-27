using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        public async Task SendPushTransferSend(SendPushTransferSendRequest request)
        {
            _logger.LogInformation("Executing SendPushTransferSend for clientId {clientId}", request.ClientId);
            
            await SendPush(NotificationTypeEnum.SendTransfer, request.ClientId, 
                s => s
                    .Replace("${AMOUNT}", request.Amount.ToString(CultureInfo.InvariantCulture))
                    .Replace("${ASSET_SYMBOL}", request.AssetSymbol)
                    .Replace("${DESTINATION_PHONE_NUMBER}", request.DestinationPhoneNumber),
                request.SenderPhoneNumber
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