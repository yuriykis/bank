using System.Threading;
using System.Threading.Tasks;
using api.Commands.TransactionCommands;
using api.Services;
using MediatR;

namespace api.Handlers
{
    public class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand, bool>
    {
        private TransactionService _transactionService;

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

            request.transaction.id = request.Id;
            _transactionService.Update(request.Id, request.transaction);

            return true;
        }
    }
}