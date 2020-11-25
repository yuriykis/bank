using System;
using api.Models;
using MediatR;

namespace api.Commands.TransactionCommands
{
    public class UpdateTransactionCommand : IRequest<bool>
    {
        public String Id { get; set; }
        public Transaction transaction { get; set; }

        public UpdateTransactionCommand(string id, Transaction transaction)
        {
            Id = id;
            this.transaction = transaction;
        }
    }
}