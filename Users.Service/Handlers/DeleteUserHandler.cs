using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Users.Service.Commands;
using Users.Service.Services;

namespace Users.Service.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly UserService _userService;

        public DeleteUserHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userService.Get(request.Id);

            if (user == null)
            {
                return false;
            }

            _userService.Remove(user.Id);

            return true;
        }
    }
}