using System;
using Accounts.Service.Models;
using MediatR;

namespace Accounts.Service.Queries
{
    public class GetAccountByIdQuery : IRequest<Account>
    {
        public GetAccountByIdQuery(string id)
        {
            this.Id = id;
        }

        public String Id { get; }
    }
}