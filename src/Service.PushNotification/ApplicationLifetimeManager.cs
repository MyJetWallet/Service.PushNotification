using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using Service.PushNotification.Grpc;
using Service.PushNotification.Jobs;
using Service.PushNotification.Services;

namespace Service.PushNotification
{
    public class ApplicationLifetimeManager : ApplicationLifetimeManagerBase
    {
        private readonly ILogger<ApplicationLifetimeManager> _logger;
        private readonly IFirebaseNotificationSender _firebaseSender;
        private readonly ITemplateService _templateService;
        private readonly TokenCleaner _cleaner;
        
        public ApplicationLifetimeManager(IHostApplicationLifetime appLifetime, ILogger<ApplicationLifetimeManager> logger, IFirebaseNotificationSender firebaseSender, ITemplateService templateService, TokenCleaner cleaner)
            : base(appLifetime)
        {
            _logger = logger;
            _firebaseSender = firebaseSender;
            _templateService = templateService;
            _cleaner = cleaner;
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");
            _firebaseSender.Start();
            _templateService.CreateDefaultTemplates();
            _cleaner.Start();
        }

        protected override void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
            _cleaner.Stop();
        }

        protected override void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");
        }
    }
}
