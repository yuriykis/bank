using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using api.Commands.TransactionCommands;
using api.Services;
using MediatR;

namespace api.Handlers
{
    public class DeleteTransactionHandler : IRequestHandler<DeleteTransactionCommand, bool>
    {
        private TransactionService _transactionService;

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

            _transactionService.Remove(transaction.id);

            return true;
        }
    }
}