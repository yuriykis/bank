using System.Threading.Tasks;
using Transactions.Service.Models;

namespace Transactions.Service.Services
{
    public interface ITransactionUpdateService
    {
        Task UpdateTransactionStatus(TransactionUpdateModel transactionUpdateModel);

    }
}