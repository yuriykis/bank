using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Users.Service.Commands;
using Users.Service.Messaging.Sender;
using Users.Service.Models;
using Users.Service.Services;

namespace Users.Service.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly UserService _userService;
        private readonly IUserAccountDeleteSender _userAccountDeleteSender;

        public DeleteUserHandler(UserService userService, IUserAccountDeleteSender userAccountDeleteSender)
        {
            _userService = userService;
            _userAccountDeleteSender = userAccountDeleteSender;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.Get(request.Id);

            if (user == null)
            {
                return false;
            }

            _userService.Remove(user.Id);
            _userAccountDeleteSender.SendDeleteUserMessage(
                new UserMessageModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Message = "DeleteAccount"
                });
            
            return true;
        }
    }
}