using System;
using MediatR;
using Users.Service.Models;

namespace Users.Service.Commands
{
    public class CreateUserCommand : IRequest<User>
    {
        public String Name { get; set; }
        public String Password { get; set;}
    }
}