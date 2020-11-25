using System;
using MediatR;

namespace api.Commands
{
    public class CreateUserCommand : IRequest<int>
    {
        public String name { get; set; }
        public String password { get; set;}
    }
}