using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Transactions.Service.Models;

namespace Transactions.Service.Services
{
    public class TransactionService
    {
        private readonly IMongoCollection<Transaction> _transactions;

        public TransactionService(IBankDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _transactions = database.GetCollection<Transaction>(settings.TransactionsCollectionName);
        }

        public async Task<List<Transaction>> Get()
        {
            var response = await _transactions.FindAsync<Transaction>(transaction => true);
            return response.ToList();
        }

        public async Task<Transaction> Get(string id)
        {
            var response = await _transactions.FindAsync<Transaction>(transaction => transaction.Id == id);
            return response.FirstOrDefault();
        }

        public async Task<Transaction> Create(Transaction transaction)
        {
            await _transactions.InsertOneAsync(transaction);
            return transaction;
        }

        public async void Update(string id, Transaction transactionIn) =>
            await _transactions.ReplaceOneAsync(transaction => transaction.Id == id, transactionIn);

        public async void Remove(Transaction transactionIn) =>
            await _transactions.DeleteOneAsync(transaction => transaction.Id == transactionIn.Id);

        public async void Remove(string id) =>
            await _transactions.DeleteOneAsync(transaction => transaction.Id == id);
    }
}
