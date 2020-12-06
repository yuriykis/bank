using Transactions.Service.Models;

namespace Transactions.Service.Messaging.Sender
{
    public interface ITransactionExecuteSender
    {
        void SendTransaction(TransactionMessageModel transaction);
    }
}