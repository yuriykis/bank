using System;
using MediatR;
using Users.Service.Authorization.Models;
using Users.Service.Models;

namespace Users.Service.Commands
{
    public class AuthenticateUserCommand : IRequest<AuthenticateResponse>
    {
        public string Username { get; set; }
        public String Password { get; set;}
    }
}