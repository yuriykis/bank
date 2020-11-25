using System.Threading;
using System.Threading.Tasks;
using api.Commands.AccountCommands;
using api.Services;
using MediatR;

namespace api.Handlers
{
    public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, bool>
    {
        private AccountService _accountService;

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

            request.account.id = request.Id;
            _accountService.Update(request.Id, request.account);

            return true;
        }
    }
}