using System.Threading.Tasks;
using Accounts.Service.Models;

namespace Accounts.Service.Services
{
    public interface IAccountUpdateService
    {
        Task UpdateAccountsAmount(AccountUpdateModel accountsUpdateModel);
        Task DeleteAccount(AccountUpdateModel accountUpdateModel);
    }
}