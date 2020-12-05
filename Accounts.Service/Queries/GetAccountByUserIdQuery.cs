using System;
using Accounts.Service.Models;
using MediatR;

namespace Accounts.Service.Queries
{
    public class GetAccountByUserIdQuery : IRequest<Account>
    {
        public GetAccountByUserIdQuery(string id)
        {
            this.Id = id;
        }

        public String Id { get; }
    }
}