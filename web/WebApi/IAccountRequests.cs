using System.Threading.Tasks;
using web.Authorization.Models;
using web.Models;

namespace web.WebApi
{
    public interface IAccountRequests
    {
        public Task<AccountModel> GetUserAccountData(string token, string userId);
    }
}