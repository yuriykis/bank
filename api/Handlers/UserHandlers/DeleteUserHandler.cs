using System.Threading;
using System.Threading.Tasks;
using api.Commands;
using api.Services;
using MediatR;

namespace api.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private UserService _userService;

        public DeleteUserHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var User = _userService.Get(request.Id);

            if (User == null)
            {
                return false;
            }

            _userService.Remove(User.id);

            return true;
        }
    }
}