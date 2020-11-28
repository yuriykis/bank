using System.Threading;
using System.Threading.Tasks;
using Accounts.Service.Commands;
using Accounts.Service.Services;
using MediatR;

namespace Accounts.Service.Handlers
{
    public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, bool>
    {
        private readonly AccountService _accountService;

        public UpdateAccountHandler(AccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<bool> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = _accountService.Get(request.Id);

            if (account == null)
            {
                return false;
            }

            request.Account.Id = request.Id;
            _accountService.Update(request.Id, request.Account);

            return true;
        }
    }
}