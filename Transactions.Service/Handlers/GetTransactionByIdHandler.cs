using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Transactions.Service.Models;
using Transactions.Service.Queries.TransactionQueries;
using Transactions.Service.Services;

namespace Transactions.Service.Handlers
{
    public class GetTransactionByIdHandler : IRequestHandler<GetTransactionByIdQuery, Transaction>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetTransactionByIdHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<Transaction> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var transactionService = scopedServices.GetRequiredService<TransactionService>();
                
                var response = await transactionService.Get();
                var transaction = response.Find(w => w.Id == request.Id);
            
                return transaction;
            }
        }
    }
}