using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Transactions.Service.Models;
using Transactions.Service.Queries.TransactionQueries;
using Transactions.Service.Services;

namespace Transactions.Service.Handlers
{
    public class GetTransactionByIdHandler : IRequestHandler<GetTransactionByIdQuery, Transaction>
    {
        private readonly TransactionService _transactionService;

        public GetTransactionByIdHandler(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<Transaction> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _transactionService.Get();
            var transaction = response.Find(w => w.Id == request.Id);
            
            return transaction;
        }
    }
}