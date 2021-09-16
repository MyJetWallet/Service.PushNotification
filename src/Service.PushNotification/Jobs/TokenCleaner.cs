using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Authorization.NoSql;
using MyJetWallet.Sdk.Service.Tools;
using MyNoSqlServer.Abstractions;
using Service.PushNotification.Domain.NoSql;

namespace Service.PushNotification.Jobs
{
    public class TokenCleaner: IStartable, IDisposable
    {
        private readonly IMyNoSqlServerDataReader<ShortRootSessionNoSqlEntity> _sessionReader;
        private readonly IMyNoSqlServerDataWriter<TokenNoSqlEntity> _tokenWriter;
        private readonly MyTaskTimer _timer;
        private readonly ILogger<TokenCleaner> _logger;

        public TokenCleaner(IMyNoSqlServerDataReader<ShortRootSessionNoSqlEntity> sessionReader, IMyNoSqlServerDataWriter<TokenNoSqlEntity> tokenWriter, ILogger<TokenCleaner> logger)
        {
            _sessionReader = sessionReader;
            _tokenWriter = tokenWriter;
            _timer = MyTaskTimer.Create<TokenCleaner>(TimeSpan.FromSeconds(Program.Settings.TimerPeriodInSec),logger,DoProcess);
            _logger = logger;
            _sessionReader.SubscribeToUpdateEvents(HandleUpdate, HandleDelete);
        }

        public void Start()
        {
            _logger.LogInformation("TokenCleaner job started");
            _timer.Start();
        }
        
        public void Stop()
        {
            _timer.Stop();
        }
        
        public void Dispose()
        {
            _timer.Dispose();
        }

        private void HandleUpdate(IReadOnlyList<ShortRootSessionNoSqlEntity> sessions)
        {
        }
        
        private async void HandleDelete(IReadOnlyList<ShortRootSessionNoSqlEntity> sessions)
        {
            var tokens = await _tokenWriter.GetAsync();
            var tasks = tokens.Select(token => Task.Run(() => ProcessEventChanges(token, sessions)));
            await Task.WhenAll(tasks);
        }
        
        private async Task DoProcess()
        {
            var sessions = _sessionReader.Get();
            if (sessions.Count > 0)
            {
                var tokens = await _tokenWriter.GetAsync();
                var tasks = tokens.Select(token => Task.Run(() => ProcessChanges(token, sessions)));
                await Task.WhenAll(tasks);
            }
        }

        private async Task ProcessEventChanges(TokenNoSqlEntity token, IReadOnlyList<ShortRootSessionNoSqlEntity> sessions)
        {
            try
            {
                if (sessions.Any(s => s.RootSessionId().ToString("N") == token.PushToken.RootSessionId))
                {
                    await _tokenWriter.DeleteAsync(token.PartitionKey, token.RowKey);
                    _logger.LogInformation("Removing firebase pushtoken with id {Id} for rootSession {RootSessionId}",token.PushToken.ClientId, token.PushToken.RootSessionId);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "When trying to delete unused push token with PartitionKey {PartitionKey}, RowKey {RowKey}", token.PartitionKey, token.RowKey);
                throw;
            }
        }
        
        private async Task ProcessChanges(TokenNoSqlEntity token, IReadOnlyList<ShortRootSessionNoSqlEntity> sessions)
        {
            try
            {
                if (sessions.All(s => s.RootSessionId().ToString("N") != token.PushToken.RootSessionId))
                {
                    await _tokenWriter.DeleteAsync(token.PartitionKey, token.RowKey);
                    _logger.LogInformation("Removing firebase pushtoken with id {Id} for rootSession {RootSessionId}",token.PushToken.ClientId, token.PushToken.RootSessionId);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "When trying to delete unused push token with PartitionKey {PartitionKey}, RowKey {RowKey}", token.PartitionKey, token.RowKey);
                throw;
            }
        }

    }
}