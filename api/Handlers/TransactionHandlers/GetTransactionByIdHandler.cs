using System.Threading;
using System.Threading.Tasks;
using api.Models;
using api.Queries;
using api.Services;
using MediatR;

namespace api.Handlers
{
    public class GetTransactionByIdHandler : IRequestHandler<GetTransactionByIdQuery, Transaction>
    {
        private TransactionService _transactionService;

        public GetTransactionByIdHandler(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<Transaction> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            Transaction transaction = _transactionService.Get().Find(w => w.id == request.Id);
            if (transaction == null)
            {
                return null;
            } 
            return transaction;
        }
    }
}