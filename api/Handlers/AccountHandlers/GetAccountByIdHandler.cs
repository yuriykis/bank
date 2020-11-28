using System.Threading;
using System.Threading.Tasks;
using api.Models;
using api.Queries;
using api.Services;
using MediatR;

namespace api.Handlers
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, Account>
    {
        private AccountService _accountService;

        public GetAccountByIdHandler(AccountService accountService)
        {
            _accountService = accountService;
        }
        

        public async Task<Account> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = _accountService.Get(request.Id);
            if (account == null)
            {
                return null;
            } 
            return account;
        }
    }
}