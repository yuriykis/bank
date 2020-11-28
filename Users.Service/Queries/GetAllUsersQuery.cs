using System.Collections.Generic;
using MediatR;
using Users.Service.Models;

namespace Users.Service.Queries
{
    public class GetAllUsersQuery : IRequest<List<User>>
    {
        
    }
}