using System.Threading;
using System.Threading.Tasks;
using Accounts.Service.Commands;
using Accounts.Service.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounts.Service.Handlers
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, bool>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DeleteAccountHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var accountService = scopedServices.GetRequiredService<AccountService>();
                
                var account = await accountService.Get(request.Id);

                if (account == null)
                {
                    return false;
                }

                await accountService.Remove(account.Id);

                return true;
            }
           
        }
    }
}