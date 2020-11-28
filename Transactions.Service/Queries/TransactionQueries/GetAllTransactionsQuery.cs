using System.Collections.Generic;
using MediatR;
using Transactions.Service.Models;

namespace Transactions.Service.Queries.TransactionQueries
{
    public class GetAllTransactionsQuery : IRequest<List<Transaction>>
    {
        
    }
}