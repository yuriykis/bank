using System;
using MediatR;
using Transactions.Service.Models;

namespace Transactions.Service.Commands
{
    public class UpdateTransactionCommand : IRequest<bool>
    {
        public String Id { get; set; }
        public Transaction Transaction { get; set; }

        public UpdateTransactionCommand(string id, Transaction transaction)
        {
            Id = id;
            this.Transaction = transaction;
        }
    }
}