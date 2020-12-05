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

        public async Task UpdateAccountsAmount(AccountUpdateModel accountUpdateModel)
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
    }
}