using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Transactions.Service.Commands;
using Transactions.Service.Services;

namespace Transactions.Service.Handlers
{
    public class DeleteTransactionHandler : IRequestHandler<DeleteTransactionCommand, bool>
    {
        private readonly TransactionService _transactionService;

        public DeleteTransactionHandler(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<bool> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = _transactionService.Get(request.Id);

            if (transaction == null)
            {
                return false;
            }

            _transactionService.Remove(transaction.Id);

            return true;
        }
    }
}