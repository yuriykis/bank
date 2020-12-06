using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MediatR;
using Transactions.Service.Commands;
using Transactions.Service.Models;
using Transactions.Service.Queries.TransactionQueries;

namespace Transactions.Service.Services
{
    public class TransactionUpdateService : ITransactionUpdateService
    {
        private readonly IMediator _mediator;

        public TransactionUpdateService(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task UpdateTransactionStatus(TransactionUpdateModel transactionUpdateModel)
        {

            try
            {
                var transaction =
                    await _mediator.Send(new GetTransactionByIdQuery(transactionUpdateModel.TransactionId));
                
                transaction.Info = transactionUpdateModel.Info;
                transaction.Status = transactionUpdateModel.Status;
                
                await _mediator.Send(new UpdateTransactionCommand(transactionUpdateModel.TransactionId, transaction));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
    }
}