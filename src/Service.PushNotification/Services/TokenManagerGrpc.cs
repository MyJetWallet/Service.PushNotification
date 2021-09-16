using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Authorization.NoSql;
using MyNoSqlServer.Abstractions;
using Service.PushNotification.Domain.Models;
using Service.PushNotification.Domain.NoSql;
using Service.PushNotification.Grpc;
using Service.PushNotification.Grpc.Models;
using Service.PushNotification.Grpc.Models.Requests;
using Service.PushNotification.Grpc.Models.Responses;

namespace Service.PushNotification.Services
{
    public class TokenManagerGrpc : ITokenManager
    {

        private readonly TokenManager _tokenManager;

        public TokenManagerGrpc(TokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public async Task RegisterToken(PushToken request) => await _tokenManager.RegisterToken(request);

        public async Task<GetUserTokensResponse> GetUserTokens(GetUserTokensRequest request) =>
            await _tokenManager.GetUserTokens(request);

        public async Task<GetAllTokensResponse> GetAllTokens(GetAllTokensRequest request) =>
            await _tokenManager.GetAllTokens(request);

        public async Task RemoveToken(RemoveTokenRequest request) => await _tokenManager.RemoveToken(request);
    }
}