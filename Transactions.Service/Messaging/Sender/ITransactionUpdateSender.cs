using Transactions.Service.Models;

namespace Transactions.Service.Messaging.Sender
{
    public interface ITransactionUpdateSender
    {
        void SendTransaction(TransactionMessageModel transaction);
    }
}