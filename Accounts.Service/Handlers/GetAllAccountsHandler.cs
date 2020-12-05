using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Service.Models;
using Accounts.Service.Queries;
using Accounts.Service.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounts.Service.Handlers
{
    public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsQuery, List<Account>>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetAllAccountsHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task<List<Account>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var accountService = scopedServices.GetRequiredService<AccountService>();
                return await accountService.Get();
            }
        }
    }
}