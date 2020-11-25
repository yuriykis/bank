using System.Threading;
using System.Threading.Tasks;
using api.Commands.AccountCommands;
using api.Services;
using MediatR;

namespace api.Handlers
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, bool>
    {
        private AccountService _accountService;

        public DeleteAccountHandler(AccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var account = _accountService.Get(request.Id);

            if (account == null)
            {
                return false;
            }

            _accountService.Remove(account.id);

            return true;
        }
    }
}