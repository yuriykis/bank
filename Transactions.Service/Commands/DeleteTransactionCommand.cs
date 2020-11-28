using System;
using MediatR;

namespace Transactions.Service.Commands
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