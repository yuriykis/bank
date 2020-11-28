using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Transactions.Service.Models;
using Transactions.Service.Queries.TransactionQueries;
using Transactions.Service.Services;

namespace Transactions.Service.Handlers
{
    public class GetAllTransactionsHandler : IRequestHandler<GetAllTransactionsQuery, List<Transaction>>
    {
        private readonly TransactionService _transactionService;

        public GetAllTransactionsHandler(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<List<Transaction>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await _transactionService.Get();
        }
    }
}