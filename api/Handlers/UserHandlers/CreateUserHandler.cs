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
            string name = request.name;
            string password = request.password;
            User user = _userService.Get().Find(w => w.name == name);
            if (user == null)
            {
                User newUser = new User { name = name, password = password };
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