using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Accounts.Service.Commands;
using Accounts.Service.Models;
using Accounts.Service.Queries;
using MediatR;

namespace Accounts.Service.Services
{
    public class AccountsAmountUpdateService : IAccountsAmountUpdateService
    {
        private readonly IMediator _mediator;

        public AccountsAmountUpdateService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task UpdateAccountsAmount(AccountsAmountUpdateModel accountsAmountUpdateModel)
        {
            try
            {
                var senderAccount = await _mediator.Send(
                    new GetAccountByIdQuery(accountsAmountUpdateModel.SenderAccountId)
                    );
                var receiverAccount = await _mediator.Send(
                    new GetAccountByIdQuery(accountsAmountUpdateModel.ReciverAccountId)
                    );

                if (senderAccount != null || receiverAccount != null)
                {
                    if (senderAccount.Amount >= accountsAmountUpdateModel.Amount)
                    {
                        senderAccount.Amount -= accountsAmountUpdateModel.Amount;
                        receiverAccount.Amount += accountsAmountUpdateModel.Amount;
                    
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
}