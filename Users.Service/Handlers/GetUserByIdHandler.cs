using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Users.Service.Models;
using Users.Service.Queries;
using Users.Service.Services;

namespace Users.Service.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly UserService _userService;

        public GetUserByIdHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _userService.Get().Find(w => w.Id == request.Id);
            if (user == null)
            {
                return null;
            } 
            return user;
        }
    }
}