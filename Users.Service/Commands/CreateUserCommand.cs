using System;
using MediatR;

namespace Users.Service.Commands
{
    public class CreateUserCommand : IRequest<int>
    {
        public String Name { get; set; }
        public String Password { get; set;}
    }
}