using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Accounts.Service.Commands;
using Accounts.Service.Messaging.Sender;
using Accounts.Service.Models;
using Accounts.Service.Queries;
using MediatR;

namespace Accounts.Service.Services
{
    public class AccountUpdateService : IAccountUpdateService
    {
        private readonly IMediator _mediator;
        private readonly ITransactionUpdateSender _transactionUpdateSender;

        public AccountUpdateService(IMediator mediator, ITransactionUpdateSender transactionUpdateSender)
        {
            _mediator = mediator;
            _transactionUpdateSender = transactionUpdateSender;
        }

        private async Task<bool> IsTransactionPermitted(AccountUpdateModel accountUpdateModel)
        {
            var senderAccountByUserId = await _mediator.Send(
                new GetAccountByUserIdQuery(accountUpdateModel.UserId));
            var senderAccountBySenderId = await _mediator.Send(
                new GetAccountByIdQuery(accountUpdateModel.SenderAccountId));
            return senderAccountByUserId.Id == senderAccountBySenderId.Id;
        }
        public async Task UpdateAccountsAmount(AccountUpdateModel accountUpdateModel)
        {
            var transactionMessageModel = new TransactionMessageModel
            {
                TransactionId = accountUpdateModel.TransactionId,
                SenderAccountId = accountUpdateModel.SenderAccountId,
                ReceiverAccountId = accountUpdateModel.ReceiverAccountId,
                Amount = accountUpdateModel.Amount
            };
            
            if (await IsTransactionPermitted(accountUpdateModel))
            {
                try
                {
                    var senderAccount = await _mediator.Send(
                        new GetAccountByIdQuery(accountUpdateModel.SenderAccountId)
                    );
                    var receiverAccount = await _mediator.Send(
                        new GetAccountByIdQuery(accountUpdateModel.ReceiverAccountId)
                    );
                    
                    if (senderAccount != null && receiverAccount != null)
                    {
                        if (senderAccount.Amount >= accountUpdateModel.Amount)
                        {
                            senderAccount.Amount -= accountUpdateModel.Amount;
                            receiverAccount.Amount += accountUpdateModel.Amount;
                    
                            await _mediator.Send(new UpdateAccountCommand(senderAccount.Id, senderAccount));
                            await _mediator.Send(new UpdateAccountCommand(receiverAccount.Id, receiverAccount));
                            transactionMessageModel.Status = "Finished";
                            transactionMessageModel.Info = "The transaction was successful";
                            _transactionUpdateSender.UpdateTransaction(transactionMessageModel);
                        }
                        else
                        {
                            transactionMessageModel.Status = "Failed";
                            transactionMessageModel.Info = "The account balance is too low";
                            _transactionUpdateSender.UpdateTransaction(transactionMessageModel);
                        }
                    }
               
                }
                catch (Exception ex)
                {
                    transactionMessageModel.Status = "Failed";
                    transactionMessageModel.Info = "Account number not found. The operation has failed";
                    _transactionUpdateSender.UpdateTransaction(transactionMessageModel);
                }
            }
            else
            {
                transactionMessageModel.Status = "Failed";
                transactionMessageModel.Info = "Operation prohibited";
                _transactionUpdateSender.UpdateTransaction(transactionMessageModel);
            }
        }

        public async Task DeleteAccount(AccountUpdateModel accountUpdateModel)
        {
            try
            {
                var accountToDelete = await _mediator.Send(
                    new GetAccountByUserIdQuery(accountUpdateModel.UserId)
                );
                await _mediator.Send(
                    new DeleteAccountCommand(accountToDelete.Id));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task CreateAccount(AccountUpdateModel accountUpdateModel)
        {
            try
            {
                await _mediator.Send(new CreateAccountCommand
                {
                    Amount = 3000,
                    UserId = accountUpdateModel.UserId
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}