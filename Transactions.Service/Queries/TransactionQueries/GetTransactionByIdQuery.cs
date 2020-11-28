using System;
using MediatR;
using Transactions.Service.Models;

namespace Transactions.Service.Queries.TransactionQueries
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