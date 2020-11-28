using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Transactions.Service.Commands;
using Transactions.Service.Services;

namespace Transactions.Service.Handlers
{
    public class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand, bool>
    {
        private readonly TransactionService _transactionService;

        public UpdateTransactionHandler(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<bool> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var user = _transactionService.Get(request.Id);

            if (user == null)
            {
                return false;
            }

            request.Transaction.Id = request.Id;
            _transactionService.Update(request.Id, request.Transaction);

            return true;
        }
    }
}