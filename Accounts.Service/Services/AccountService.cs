using System.Collections.Generic;
using System.Threading.Tasks;
using Accounts.Service.Models;
using MongoDB.Driver;

namespace Accounts.Service.Services
{
    public class AccountService
    {
        private readonly IMongoCollection<Account> _accounts;

        public AccountService(IBankDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _accounts = database.GetCollection<Account>(settings.AccountsCollectionName);
        }

        public async Task<List<Account>> Get()
        {
            var response = await _accounts.FindAsync(account => true);
            return response.ToList();
        }

        public async Task<Account> Get(string id)
        {
            var response = await _accounts.FindAsync(account => account.Id == id);
            return response.FirstOrDefault();
        }

        public async Task<Account> Create(Account account)
        {
            await _accounts.InsertOneAsync(account);
            return account;
        }

        public async void Update(string id, Account accountIn) =>
            await _accounts.ReplaceOneAsync(account => account.Id == id, accountIn);

        public async void Remove(Account accountIn) =>
            await _accounts.DeleteOneAsync(account => account.Id == accountIn.Id);

        public async void Remove(string id) =>
            await _accounts.DeleteOneAsync(account => account.Id == id);
    }
}
