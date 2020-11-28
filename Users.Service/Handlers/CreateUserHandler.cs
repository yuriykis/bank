using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Users.Service.Commands;
using Users.Service.Models;
using Users.Service.Services;

namespace Users.Service.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly UserService _userService;

        public CreateUserHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var name = request.Name;
            var password = request.Password;
            var user = _userService.Get().Find(w => w.Name == name);
            if (user == null)
            {
                var newUser = new User { Name = name, Password = password };
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