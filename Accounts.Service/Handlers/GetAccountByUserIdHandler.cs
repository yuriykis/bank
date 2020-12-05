using System.Threading;
using System.Threading.Tasks;
using Accounts.Service.Models;
using Accounts.Service.Queries;
using Accounts.Service.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounts.Service.Handlers
{
    public class GetAccountByUserIdHandler : IRequestHandler<GetAccountByUserIdQuery, Account>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public GetAccountByUserIdHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        
        public async Task<Account> Handle(GetAccountByUserIdQuery request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var accountService = scopedServices.GetRequiredService<AccountService>();
                return await accountService.GetByUserId(request.Id);
            }
        }
    }
}