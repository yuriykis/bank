using System;
using MediatR;
using Users.Service.Authorization.Models;
using Users.Service.Models;

namespace Users.Service.Commands
{
    public class AuthenticateUserCommand : IRequest<AuthenticateResponse>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public String Password { get; set;}
    }
}