using System.Threading;
using System.Threading.Tasks;
using api.Commands;
using api.Services;
using MediatR;

namespace api.Handlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private UserService _userService;

        public UpdateUserHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userService.Get(request.Id);

            if (user == null)
            {
                return false;
            }

            request.user.id = request.Id;
            _userService.Update(request.Id, request.user);

            return true;
        }
    }
}