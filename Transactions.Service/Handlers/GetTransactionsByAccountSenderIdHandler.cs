using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Transactions.Service.Models;
using Transactions.Service.Queries.TransactionQueries;
using Transactions.Service.Services;

namespace Transactions.Service.Handlers
{
    public class GetTransactionsByAccountSenderIdHandler : IRequestHandler<GetTransactionsByAccountSenderIdQuery, List<Transaction>>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetTransactionsByAccountSenderIdHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<List<Transaction>> Handle(GetTransactionsByAccountSenderIdQuery request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var transactionService = scopedServices.GetRequiredService<TransactionService>();

                return await transactionService.GetBySenderId(request.AccountId);
            }
        }
    }
}