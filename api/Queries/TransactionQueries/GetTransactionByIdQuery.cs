using System;
using api.Models;
using MediatR;

namespace api.Queries
{
    public class GetTransactionByIdQuery : IRequest<Transaction>
    {
        public GetTransactionByIdQuery(string id)
        {
            this.Id = id;
        }

        public String Id { get; }
    }
}