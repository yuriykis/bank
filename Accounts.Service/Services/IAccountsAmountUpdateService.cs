using System.Threading.Tasks;
using Accounts.Service.Models;

namespace Accounts.Service.Services
{
    public interface IAccountsAmountUpdateService
    {
        Task UpdateAccountsAmount(AccountsAmountUpdateModel accountsAmountUpdateModel);
    }
}