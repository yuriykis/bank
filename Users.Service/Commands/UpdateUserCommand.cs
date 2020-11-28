using System;
using MediatR;
using Users.Service.Models;

namespace Users.Service.Commands
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public String Id { get; set; }
        public User User { get; set; }

        public UpdateUserCommand(string id, User user)
        {
            Id = id;
            this.User = user;
        }
    }
}