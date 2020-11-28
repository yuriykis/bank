using MediatR;

namespace Transactions.Service.Commands
{
    public class CreateTransactionCommand : IRequest<int>
    {
        public string SenderAccountId { get; set; }
        
        public string ReciverAccountId { get; set; }
        
        public long amount { get; set; }
    }
}