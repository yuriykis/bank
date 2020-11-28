using System.Threading;
using System.Threading.Tasks;
using api.Models;
using api.Queries;
using api.Services;
using MediatR;

namespace api.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private UserService _userService;

        public GetUserByIdHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _userService.Get().Find(w => w.id == request.Id);
            if (user == null)
            {
                return null;
            } 
            return user;
        }
    }
}