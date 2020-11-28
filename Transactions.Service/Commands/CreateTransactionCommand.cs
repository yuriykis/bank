using MediatR;

namespace Transactions.Service.Commands
{
    public class CreateTransactionCommand : IRequest<int>
    {
        public string SenderAccountId { get; set; }
        
        public string ReceiverAccountId { get; set; }
        
        public long Amount { get; set; }
    }
}