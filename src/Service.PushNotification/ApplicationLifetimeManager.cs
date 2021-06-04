using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using Service.PushNotification.Services;

namespace Service.PushNotification
{
    public class ApplicationLifetimeManager : ApplicationLifetimeManagerBase
    {
        private readonly ILogger<ApplicationLifetimeManager> _logger;
        private readonly IFirebaseNotificationSender _firebaseSender;

        public ApplicationLifetimeManager(IHostApplicationLifetime appLifetime, ILogger<ApplicationLifetimeManager> logger, IFirebaseNotificationSender firebaseSender)
            : base(appLifetime)
        {
            _logger = logger;
            _firebaseSender = firebaseSender;
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");
            _firebaseSender.Start();
        }

        protected override void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
        }

        protected override void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");
        }
    }
}
