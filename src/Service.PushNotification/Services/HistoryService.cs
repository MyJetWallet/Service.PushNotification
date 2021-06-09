using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.PushNotification.Grpc;
using Service.PushNotification.Grpc.Models;
using Service.PushNotification.Grpc.Models.Requests;
using Service.PushNotification.Grpc.Models.Responses;
using Service.PushNotification.Postgres;

namespace Service.PushNotification.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRecordingService _historyRecordingService;

        public HistoryService(IHistoryRecordingService historyRecordingService)
        {
            _historyRecordingService = historyRecordingService;
        }

        public async Task<HistoryListResponse> GetAllRecords(HistoryRequest request) => HistoryEntityToResponseList(await _historyRecordingService.GetAllRecords(request.TimeStamp, request.Take));

        public async Task<HistoryListResponse> GetRecordsByClientId(HistoryRequest request) => HistoryEntityToResponseList(await _historyRecordingService.GetRecordsByClientId(request.ClientId, request.TimeStamp, request.Take ));

        public async Task<HistoryResponse> GetRecordByMessageId(HistoryRequest request) => HistoryEntityToResponse(await _historyRecordingService.GetRecordByMessageId(request.NotificationId));

        private static HistoryListResponse HistoryEntityToResponseList(List<NotificationHistoryDbEntity> entities) =>
            new HistoryListResponse()
            {
                HistoryResponses = entities?.Select(HistoryEntityToResponse).ToList()
            };

        private static HistoryResponse HistoryEntityToResponse(NotificationHistoryDbEntity entity) =>
            entity == null 
                ? new HistoryResponse()
                : new HistoryResponse()
                {
                    ClientId = entity.ClientId,
                    NotificationId = entity.NotificationId,
                    Params = entity.Params,
                    Type = entity.Type,
                    StatusResponses = StatusEntityToResponse(entity.DeliveryStatuses),
                    TimeStamp = entity.TimeStamp
                };

        private static List<StatusResponse> StatusEntityToResponse(List<NotificationStatusDbEntity> entities) =>
            entities?.Where(p=>p != null).Select(entity => new StatusResponse
                {IsSuccess = entity.IsSuccess, StatusId = entity.StatusId, Token = entity.Token, UserAgent = entity.UserAgent}).ToList();
    }
}