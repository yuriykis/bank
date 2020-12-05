using System;
using MediatR;
using Users.Service.Models;

namespace Users.Service.Commands
{
    public class CreateUserCommand : IRequest<User>
    {
        public string Username { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public String Password { get; set;}
    }
}