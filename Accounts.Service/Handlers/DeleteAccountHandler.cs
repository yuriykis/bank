using System.Threading;
using System.Threading.Tasks;
using Accounts.Service.Commands;
using Accounts.Service.Services;
using MediatR;

namespace Accounts.Service.Handlers
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, bool>
    {
        private readonly AccountService _accountService;

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

            _accountService.Remove(account.Id);

            return true;
        }
    }
}