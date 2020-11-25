using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;
using api.Queries;
using api.Services;
using MediatR;

namespace api.Handlers
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