using System;
using MediatR;
using Users.Service.Models;

namespace Users.Service.Queries
{
    public class GetUserByParamsQuery : IRequest<User>
    {
        public GetUserByParamsQuery(string firstName, string lastName, string password)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Password { get; set; }
    }
}