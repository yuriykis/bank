using System;
using MediatR;

namespace api.Commands.TransactionCommands
{
    public class DeleteTransactionCommand : IRequest<bool>
    {
        public DeleteTransactionCommand(string id)
        {
            Id = id;
        }

        public String Id { get; set; }
    }
}