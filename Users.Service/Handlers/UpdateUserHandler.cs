using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Users.Service.Commands;
using Users.Service.Services;

namespace Users.Service.Handlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly UserService _userService;

        public UpdateUserHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.Get(request.Id);

            if (user == null)
            {
                return false;
            }

            request.User.Id = request.Id;
            _userService.Update(request.Id, request.User);

            return true;
        }
    }
}