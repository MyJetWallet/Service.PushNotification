using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using Service.PushNotification.Domain.Models.Enums;
using Service.PushNotification.Grpc;
using Service.PushNotification.Grpc.Models;
using Service.PushNotification.Grpc.Models.Requests;

namespace Service.PushNotification.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<NotificationService> _logger;
        private readonly IFirebaseNotificationSender _firebaseSender;
        private readonly IHistoryRecordingService _historyService;

        public NotificationService(ITokenManager tokenManager, ILogger<NotificationService> logger, IFirebaseNotificationSender firebaseSender, IHistoryRecordingService historyService)
        {
            _tokenManager = tokenManager;
            _logger = logger;
            _firebaseSender = firebaseSender;
            _historyService = historyService;
        }

        public async Task SendPushLogin(LoginPushRequest request)
        {
            var tokens = await _tokenManager.GetUserTokens(new GetUserTokensRequest()
            {
                ClientId = request.ClientId
            });
            var title = $"Login {request.Ip}";
            var body = $"Login placeholder {request.Date}, {request.Ip}";

            var msgId = Guid.NewGuid();
            var parameters = new List<string>()
            {
                request.Ip,
                request.Date.ToString()
            };

            await _historyService.RecordNotification(msgId, NotificationTypeEnum.LoginNotification, request.ClientId,
                parameters);
            
            await _firebaseSender.SendNotificationPush(msgId, tokens.Tokens, title, body);
        }

        public async Task SendPushTrade(TradePushRequest request)
        {
            var tokens = await _tokenManager.GetUserTokens(new GetUserTokensRequest()
            {
                ClientId = request.ClientId
            });
            var title = $"Trade {request.Instrument} {request.Amount}";
            var body = $"Trade placeholder {request.Date}, {request.Amount}, {request.Instrument}";
            
            var msgId = Guid.NewGuid();
            var parameters = new List<string>()
            {
                request.Amount.ToString(),
                request.Date.ToString(),
                request.Instrument,
            };

            await _historyService.RecordNotification(msgId, NotificationTypeEnum.TradeNotification, request.ClientId,
                parameters);
            
            await _firebaseSender.SendNotificationPush(msgId, tokens.Tokens, title, body);        
        }
    }
}