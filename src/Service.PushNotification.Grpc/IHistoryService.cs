using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Service.PushNotification.Grpc.Models;
using Service.PushNotification.Grpc.Models.Requests;
using Service.PushNotification.Grpc.Models.Responses;

namespace Service.PushNotification.Grpc
{
    [ServiceContract]
    public interface IHistoryService
    {
        [OperationContract]
        Task<HistoryListResponse> GetAllRecords(HistoryRequest request);
        
        [OperationContract]
        Task<HistoryListResponse> GetRecordsByClientId(HistoryRequest request);
        
        [OperationContract]
        Task<HistoryResponse> GetRecordByMessageId(HistoryRequest request);
    }
}