using System;
using System.Collections.Generic;
using MediatR;
using Transactions.Service.Models;

namespace Transactions.Service.Queries.TransactionQueries
{
    public class GetTransactionsByAccountReceiverIdQuery : IRequest<List<Transaction>>
    {
        public GetTransactionsByAccountReceiverIdQuery(string id)
        {
            this.AccountId = id;
        }

        public String AccountId { get; }
    }
}