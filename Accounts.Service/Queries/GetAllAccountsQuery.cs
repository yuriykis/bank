using System.Collections.Generic;
using Accounts.Service.Models;
using MediatR;

namespace Accounts.Service.Queries
{
    public class GetAllAccountsQuery : IRequest<List<Account>>
    {
        
    }
}