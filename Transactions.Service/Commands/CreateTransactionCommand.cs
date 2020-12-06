using MediatR;
using Transactions.Service.Models;

namespace Transactions.Service.Commands
{
    public class CreateTransactionCommand : IRequest<Transaction>
    {
        public string SenderAccountId { get; set; }
        
        public string ReceiverAccountId { get; set; }

        public string UserId { get; set; }
        public long Amount { get; set; }
    }
}