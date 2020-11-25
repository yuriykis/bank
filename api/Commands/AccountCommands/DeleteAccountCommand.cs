using System;
using MediatR;

namespace api.Commands.AccountCommands
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