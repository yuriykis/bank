using Accounts.Service.Models;

namespace Accounts.Service.Messaging.Sender
{
    public interface ITransactionUpdateSender
    {
        void UpdateTransaction(TransactionMessageModel transaction);
    }
}