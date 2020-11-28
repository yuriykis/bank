using System;
using Accounts.Service.Models;
using MediatR;

namespace Accounts.Service.Commands
{
    public class UpdateAccountCommand : IRequest<bool>
    {
        public String Id { get; set; }
        public Account Account { get; set; }

        public UpdateAccountCommand(string id, Account account)
        {
            Id = id;
            this.Account = account;
        }
    }
}