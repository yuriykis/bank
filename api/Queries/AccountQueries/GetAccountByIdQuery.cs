using System;
using api.Models;
using MediatR;

namespace api.Queries
{
    public class GetAccountByIdQuery : IRequest<Account>
    {
        public GetAccountByIdQuery(string id)
        {
            this.Id = id;
        }

        public String Id { get; }
    }
}