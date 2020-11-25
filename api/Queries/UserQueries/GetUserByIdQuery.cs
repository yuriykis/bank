using System;
using api.Models;
using MediatR;

namespace api.Queries
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