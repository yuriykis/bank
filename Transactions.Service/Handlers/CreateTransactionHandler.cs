using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Transactions.Service.Commands;
using Transactions.Service.Messaging.Sender;
using Transactions.Service.Models;
using Transactions.Service.Services;

namespace Transactions.Service.Handlers
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Transaction>
    {
        private readonly ITransactionExecuteSender _transactionExecuteSender;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public CreateTransactionHandler(ITransactionExecuteSender transactionExecuteSender, IServiceScopeFactory serviceScopeFactory)
        {
            _transactionExecuteSender = transactionExecuteSender;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<Transaction> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var transactionService = scopedServices.GetRequiredService<TransactionService>();
                var senderAccountId = request.SenderAccountId;
                var receiverAccountId = request.ReceiverAccountId;
                var amount = request.Amount;
            
                var newTransaction = new Transaction
                {
                    SenderAccountId = senderAccountId, 
                    ReceiverAccountId = receiverAccountId, 
                    Amount = amount,
                    Status = "In progress",
                    Info = "Transaction is in progress"
                };

                var response = await transactionService.Create(newTransaction);

                _transactionExecuteSender.SendTransaction(new TransactionMessageModel
                {
                    TransactionId = response.Id,
                    SenderAccountId = newTransaction.SenderAccountId,
                    ReceiverAccountId = newTransaction.ReceiverAccountId,
                    Amount = newTransaction.Amount,
                    UserId = request.UserId,
                    Message = "ExecuteTransaction"
                });
            
                return newTransaction;   
                
            }
        }
    }
}