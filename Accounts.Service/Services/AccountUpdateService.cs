using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Accounts.Service.Commands;
using Accounts.Service.Models;
using Accounts.Service.Queries;
using MediatR;

namespace Accounts.Service.Services
{
    public class AccountUpdateService : IAccountUpdateService
    {
        private readonly IMediator _mediator;

        public AccountUpdateService(IMediator mediator)
        {
            _mediator = mediator;
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
                        }
                        else
                        {
                            Debug.WriteLine("Brak pieniedzy");
                        }
                    }
               
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
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