using System;
using MediatR;

namespace api.Commands
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