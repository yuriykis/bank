using System;
using MediatR;

namespace Accounts.Service.Commands
{
    public class DeleteAccountCommand : IRequest<bool>
    {
        public DeleteAccountCommand(string id)
        {
            Id = id;
        }

        public String Id { get; set; }
    }
}