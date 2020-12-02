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
            var account = await _accountService.Get(request.Id);

            request.Account.Id = request.Id;
            _accountService.Update(request.Id, request.Account);

            return true;
        }
    }
}