using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Service.PushNotification.Domain.Models;
using Service.PushNotification.Domain.NoSql;
using Service.PushNotification.Grpc;
using Service.PushNotification.Grpc.Models;
using Service.PushNotification.Grpc.Models.Requests;
using Service.PushNotification.Grpc.Models.Responses;
using ILogger = Serilog.ILogger;

namespace Service.PushNotification.Services
{
    public class TokenManager : ITokenManager
    {

        private readonly IMyNoSqlServerDataWriter<TokenNoSqlEntity> _noSqlWriter;
        private readonly ILogger<TokenManager> _logger;

        public TokenManager(IMyNoSqlServerDataWriter<TokenNoSqlEntity> noSqlWriter, ILogger<TokenManager> logger)
        {
            _noSqlWriter = noSqlWriter;
            _logger = logger;
        }


        public async Task RegisterToken(PushToken request)
        {
            try
            {
                await _noSqlWriter.InsertOrReplaceAsync(TokenNoSqlEntity.Create(request));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "When upserting user token to DB. ClientId {ClientId}, RootSessionId {RootSessionId}", request.ClientId, request.RootSessionId);
                throw;
            }

        }

        public async Task<GetUserTokensResponse> GetUserTokens(GetUserTokensRequest request)
        {
            try
            {
                var tokenEntities = await _noSqlWriter.GetAsync(request.ClientId);
                return new GetUserTokensResponse
                {
                    Tokens = tokenEntities.Select(t => t.PushToken).ToList()
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "When getting tokens from DB for user with ClientId {ClientId}", request.ClientId);
                throw;
            }
        }
        
        public async Task<GetAllTokensResponse> GetAllTokens(GetAllTokensRequest request)
        {
            try
            {
                var tokenEntities = await _noSqlWriter.GetAsync();
                return new GetAllTokensResponse
                {
                    Tokens = tokenEntities
                        .Where(t=>t.PushToken != null)
                        .Where(t=> t.CreateTime < request.TimeStamp)
                        .Take(request.Take ?? 30)
                        .Select(t => t.PushToken).ToList()
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "When getting all tokens from DB");
                throw;
            }
        }

        public async Task RemoveToken(RemoveTokenRequest request)
        {
            try
            {
                var partKey = TokenNoSqlEntity.GeneratePartitionKey(request.ClientId);
                var rowKey = TokenNoSqlEntity.GenerateRowKey(request.RootSessionId);
                await _noSqlWriter.DeleteAsync(partKey, rowKey);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"When deleting token for client {request.ClientId} and rootSession {request.RootSessionId}", request.ClientId, request.RootSessionId);
                throw;
            }

        }
    }
}