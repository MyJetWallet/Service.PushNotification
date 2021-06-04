using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using Service.PushNotification.Grpc;
using Service.PushNotification.Grpc.Models;

namespace Service.PushNotification.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<NotificationService> _logger;
        private readonly IFirebaseNotificationSender _firebaseSender;

        public NotificationService(ITokenManager tokenManager, ILogger<NotificationService> logger, IFirebaseNotificationSender firebaseSender)
        {
            _tokenManager = tokenManager;
            _logger = logger;
            _firebaseSender = firebaseSender;
        }

        public async Task SendPushLogin(LoginPushRequest request)
        {
            var tokens = await _tokenManager.GetUserTokens(new GetUserTokensRequest()
            {
                ClientId = request.ClientId
            });
            var message = $"Login message placeholder {request.Date}, {request.Ip}";
            await _firebaseSender.SendNotificationPush(tokens.Tokens, message);
        }

        public async Task SendPushTrade(TradePushRequest request)
        {
            var tokens = await _tokenManager.GetUserTokens(new GetUserTokensRequest()
            {
                ClientId = request.ClientId
            });
            var message = $"Trade message placeholder {request.Date}, {request.Amount}, {request.Instrument}";
            await _firebaseSender.SendNotificationPush(tokens.Tokens, message);        
        }
    }
}