using System.Threading.Tasks;
using web.Authorization.Models;
using web.Models;

namespace web.WebApi
{
    public interface ITransactionRequests
    {
        Task<bool> CompleteTransaction(string token, TransactionModel transactionModel);
    }
}