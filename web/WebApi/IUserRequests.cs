using System.Threading.Tasks;
using web.Authorization.Models;
using web.Models;

namespace web.WebApi
{
    public interface IUserRequests
    {
        public Task<UserModel> GetUserData(string token, string userId);
        public Task<AuthenticateResponse> LoginUser(AuthenticateRequest authenticateRequest);
        public Task<bool> RegisterUser(UserModel userModel);
    }
}