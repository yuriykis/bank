using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Service.Models;
using Accounts.Service.Queries;
using Accounts.Service.Services;
using MediatR;

namespace Accounts.Service.Handlers
{
    public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsQuery, List<Account>>
    {
        private readonly AccountService _accountService;

        public GetAllAccountsHandler(AccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<List<Account>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
        {
            return _accountService.Get();
        }
    }
}