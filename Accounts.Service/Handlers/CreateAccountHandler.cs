using System.Threading;
using System.Threading.Tasks;
using Accounts.Service.Commands;
using Accounts.Service.Models;
using Accounts.Service.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounts.Service.Handlers
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, Account>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CreateAccountHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<Account> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var accountService = scopedServices.GetRequiredService<AccountService>();

                var amount = request.Amount;
                var userId = request.UserId;
                
                var newAccount = new Account
                {
                    Amount = amount,
                    UserId = userId
                };
                
                var response = await accountService.Create(newAccount);
                return response;
            }
            
        }
    }
}