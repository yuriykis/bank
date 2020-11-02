using api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
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

        public List<Account> Get() =>
            _accounts.Find(account => true).ToList();

        public Account Get(string id) =>
            _accounts.Find<Account>(account => account.id == id).FirstOrDefault();

        public Account Create(Account account)
        {
            _accounts.InsertOne(account);
            return account;
        }

        public void Update(string id, Account accountIn) =>
            _accounts.ReplaceOne(account => account.id == id, accountIn);

        public void Remove(Account accountIn) =>
            _accounts.DeleteOne(account => account.id == accountIn.id);

        public void Remove(string id) =>
            _accounts.DeleteOne(account => account.id == id);
    }
}
