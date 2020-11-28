using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Users.Service.Commands;
using Users.Service.Models;
using Users.Service.Services;

namespace Users.Service.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly UserService _userService;

        public CreateUserHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var name = request.Name;
            var password = request.Password;
            var response1 = await _userService.Get();
            var user = response1.Find(w => w.Name == name);

            var newUser = new User { Name = name, Password = password };
            var response = await _userService.Create(newUser);
            return response;
        }
    }
}