using System;
using System.Collections.Generic;
using MediatR;
using Transactions.Service.Models;

namespace Transactions.Service.Queries.TransactionQueries
{
    public class GetTransactionsByAccountSenderIdQuery : IRequest<List<Transaction>>
    {
        public GetTransactionsByAccountSenderIdQuery(string id)
        {
            this.AccountId = id;
        }

        public String AccountId { get; }
    }
}