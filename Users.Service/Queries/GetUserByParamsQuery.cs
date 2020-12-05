using System;
using MediatR;
using Users.Service.Models;

namespace Users.Service.Queries
{
    public class GetUserByParamsQuery : IRequest<User>
    {
        public GetUserByParamsQuery(string username, string password)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}