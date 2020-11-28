using System.Threading;
using System.Threading.Tasks;
using api.Commands;
using api.Models;
using api.Services;
using MediatR;

namespace api.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
    {
        private UserService _userService;

        public CreateUserHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var name = request.name;
            var password = request.password;
            var user = _userService.Get().Find(w => w.name == name);
            if (user == null)
            {
                var newUser = new User { name = name, password = password };
                _userService.Create(newUser);
                return 200;
            }
            else
            {
                return 400;
            }
        }
    }
}