using System.Threading;
using System.Threading.Tasks;
using Accounts.Service.Models;
using Accounts.Service.Queries;
using Accounts.Service.Services;
using MediatR;

namespace Accounts.Service.Handlers
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, Account>
    {
        private readonly AccountService _accountService;

        public GetAccountByIdHandler(AccountService accountService)
        {
            _accountService = accountService;
        }
        

        public async Task<Account> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountService.Get(request.Id);
            return account;
        }
    }
}