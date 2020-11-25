using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;
using api.Queries;
using api.Services;
using MediatR;

namespace api.Handlers
{
    public class GetAllTransationsHandler : IRequestHandler<GetAllTransactionsQuery, List<Transaction>>
    {
        private readonly TransactionService _transactionService;

        public GetAllTransationsHandler(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<List<Transaction>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            return _transactionService.Get();
        }
    }
}