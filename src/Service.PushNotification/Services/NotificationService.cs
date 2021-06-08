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
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<NotificationService> _logger;
        private readonly IFirebaseNotificationSender _firebaseSender;
        private readonly IHistoryRecordingService _historyService;
        private readonly ITemplateService _templateService;

        public NotificationService(ITokenManager tokenManager, ILogger<NotificationService> logger,
            IFirebaseNotificationSender firebaseSender, IHistoryRecordingService historyService,
            ITemplateService templateService)
        {
            _tokenManager = tokenManager;
            _logger = logger;
            _firebaseSender = firebaseSender;
            _historyService = historyService;
            _templateService = templateService;
        }

        public async Task SendPushLogin(LoginPushRequest request)
        {
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

                var pushTokens = lang.Select(t => t.Token).ToArray();
                await _firebaseSender.SendNotificationPush(msgId, pushTokens, title, body);
            }
        }

        public async Task SendPushTrade(TradePushRequest request)
        {
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

                var pushTokens = lang.Select(t => t.Token).ToArray();
                await _firebaseSender.SendNotificationPush(msgId, pushTokens, title, body);
            }
        }
    }
}