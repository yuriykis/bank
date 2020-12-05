using Accounts.Service.Models;
using MediatR;

namespace Accounts.Service.Commands
{
    public class CreateAccountCommand : IRequest<Account>
    {
        public long Amount { get; set; }

        public string UserId { get; set; }
    }
}