using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Transactions.Service.Commands;
using Transactions.Service.Messaging.Sender;
using Transactions.Service.Models;
using Transactions.Service.Services;

namespace Transactions.Service.Handlers
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Transaction>
    {
        private readonly TransactionService _transactionService;
        private readonly ITransactionUpdateSender _transactionUpdateSender;

        public CreateTransactionHandler(TransactionService transactionService, ITransactionUpdateSender transactionUpdateSender)
        {
            _transactionService = transactionService;
            _transactionUpdateSender = transactionUpdateSender;
        }

        public async Task<Transaction> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var senderAccountId = request.SenderAccountId;
            var receiverAccountId = request.ReceiverAccountId;
            var amount = request.Amount;
            
            var newTransaction = new Transaction
            {
                SenderAccountId = senderAccountId, 
                ReciverAccountId = receiverAccountId, 
                Amount = amount
            };
            
            _transactionService.Create(newTransaction);
            _transactionUpdateSender.SendTransaction(newTransaction);
            
            return newTransaction;
        }
    }
}