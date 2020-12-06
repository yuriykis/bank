using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Transactions.Service.Commands;
using Transactions.Service.Services;

namespace Transactions.Service.Handlers
{
    public class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand, bool>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UpdateTransactionHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<bool> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var transactionService = scopedServices.GetRequiredService<TransactionService>();
                
                var transaction = await transactionService.Get(request.Id);

                if (transaction == null)
                {
                    return false;
                }

                request.Transaction.Id = request.Id;
                await transactionService.Update(request.Id, request.Transaction);

                return true;
            }
        }
    }
}