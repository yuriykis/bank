using System.Collections.Generic;
using api.Models;
using MediatR;

namespace api.Queries
{
    public class GetAllAccountsQuery : IRequest<List<Account>>
    {
        
    }
}