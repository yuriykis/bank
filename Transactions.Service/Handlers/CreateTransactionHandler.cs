using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Transactions.Service.Commands;
using Transactions.Service.Models;
using Transactions.Service.Services;

namespace Transactions.Service.Handlers
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, int>
    {
        private TransactionService _transactionService;

        public CreateTransactionHandler(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<int> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            String senderAccountId = request.SenderAccountId;
            String reveiverAccountId = request.ReciverAccountId;
            
            Transaction newTransaction = new Transaction { SenderAccountId = senderAccountId, ReciverAccountId = reveiverAccountId};
            _transactionService.Create(newTransaction);
            return 200;
        }
    }
}