using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Users.Service.Commands;
using Users.Service.Messaging.Sender.Create;
using Users.Service.Models;
using Users.Service.Services;

namespace Users.Service.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly UserService _userService;
        private readonly IUserAccountCreateSender _userAccountCreateSender;

        public CreateUserHandler(UserService userService, IUserAccountCreateSender userAccountCreateSender)
        {
            _userService = userService;
            _userAccountCreateSender = userAccountCreateSender;
        }

        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var username = request.Username;
            var firstName = request.FirstName;
            var lastName = request.LastName;
            var password = request.Password;
            
            var newUser = new User { Username = username, FirstName = firstName, LastName = lastName, Password = password };
            var response = await _userService.Create(newUser);
            var user = await _userService.Get(username, password);
            
            _userAccountCreateSender.SendCreateUserMessage(
                new UserMessageModel
                {
                    UserId = user.Id,
                    Message = "CreateAccount"
                });
            
            return response;
        }
    }
}