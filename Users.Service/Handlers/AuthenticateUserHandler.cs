using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Users.Service.Authorization.Models;
using Users.Service.Commands;
using Users.Service.Models;
using Users.Service.Services;

namespace Users.Service.Handlers
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateResponse>
    {
        private readonly UserService _userService;

        public AuthenticateUserHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<AuthenticateResponse> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var authenticateRequest = new AuthenticateRequest
            {
                FirstName = request.FirstName, 
                LastName = request.LastName,
                Password = request.Password
            };

            var response = await _userService.Authenticate(authenticateRequest);

            return response;
        }
    }
}