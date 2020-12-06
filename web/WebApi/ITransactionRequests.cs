using System.Collections.Generic;
using System.Threading.Tasks;
using web.Authorization.Models;
using web.Models;

namespace web.WebApi
{
    public interface ITransactionRequests
    {
        Task<bool> CompleteTransaction(string token, TransactionModel transactionModel);

        Task<List<TransactionModel>> GetTransactionBySenderList(string token, string accountId);
        
        Task<List<TransactionModel>> GetTransactionByReceiverList(string token, string accountId);
    }
}