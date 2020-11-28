using System;
using MediatR;

namespace Users.Service.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public DeleteUserCommand(string id)
        {
            Id = id;
        }

        public String Id { get; set; }
    }
}