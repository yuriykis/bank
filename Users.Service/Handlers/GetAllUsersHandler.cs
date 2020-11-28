using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Users.Service.Models;
using Users.Service.Queries;
using Users.Service.Services;

namespace Users.Service.Handlers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<User>>
    {
        private readonly UserService _userService;

        public GetAllUsersHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return  _userService.Get();
        }
    }
}