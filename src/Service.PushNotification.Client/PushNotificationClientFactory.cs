using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using JetBrains.Annotations;
using MyJetWallet.Sdk.GrpcMetrics;
using ProtoBuf.Grpc.Client;
using Service.PushNotification.Grpc;

namespace Service.PushNotification.Client
{
    [UsedImplicitly]
    public class PushNotificationClientFactory
    {
        private readonly CallInvoker _channel;

        public PushNotificationClientFactory(string assetsDictionaryGrpcServiceUrl)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var channel = GrpcChannel.ForAddress(assetsDictionaryGrpcServiceUrl);
            _channel = channel.Intercept(new PrometheusMetricsInterceptor());
        }

        public ITokenManager GetTokenManager() => _channel.CreateGrpcService<ITokenManager>();
        
        public INotificationService GetNotificationService() => _channel.CreateGrpcService<INotificationService>();

        public IHistoryService GetHistoryService() => _channel.CreateGrpcService<IHistoryService>();

        public ITemplateService GetTemplateService() => _channel.CreateGrpcService<ITemplateService>();

    }
}
