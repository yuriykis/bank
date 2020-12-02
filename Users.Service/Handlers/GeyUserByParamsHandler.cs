using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Users.Service.Models;
using Users.Service.Queries;
using Users.Service.Services;

namespace Users.Service.Handlers
{
    public class GeyUserByParamsHandler : IRequestHandler<GetUserByParamsQuery, User>
    {
        private readonly UserService _userService;

        public GeyUserByParamsHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<User> Handle(GetUserByParamsQuery request, CancellationToken cancellationToken)
        {
            return await _userService.Get(request.FirstName, request.LastName, request.Password);
        }
    }
}