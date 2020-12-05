using System.Threading;
using System.Threading.Tasks;
using Accounts.Service.Commands;
using Accounts.Service.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounts.Service.Handlers
{
    public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, bool>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UpdateAccountHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<bool> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var accountService = scopedServices.GetRequiredService<AccountService>();

                request.Account.Id = request.Id;
                await accountService.Update(request.Id, request.Account);

                return true;
            }
        }
    }
}