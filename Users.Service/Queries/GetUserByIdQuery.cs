using System;
using MediatR;
using Users.Service.Models;

namespace Users.Service.Queries
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public GetUserByIdQuery(string id)
        {
            this.Id = id;
        }

        public String Id { get; }
        
        
    }
}