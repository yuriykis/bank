using System;
using api.Models;
using MediatR;

namespace api.Commands.AccountCommands
{
    public class UpdateAccountCommand : IRequest<bool>
    {
        public String Id { get; set; }
        public Account account { get; set; }

        public UpdateAccountCommand(string id, Account account)
        {
            Id = id;
            this.account = account;
        }
    }
}