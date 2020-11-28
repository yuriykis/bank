using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Users.Service.Models;

namespace Users.Service.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IBankDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public async Task<List<User>> Get()
        {
            var response = await _users.FindAsync(user => true);
            return response.ToList();
        }

        public async Task<User> Get(string id)
        {
            var response = await _users.FindAsync<User>(user => user.Id == id);
            return response.FirstOrDefault();
        }

        public async Task<User> Create(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async void Update(string id, User userIn) =>
            await _users.ReplaceOneAsync(user => user.Id == id, userIn);

        public async void Remove(User userIn) =>
            await _users.DeleteOneAsync(user => user.Id == userIn.Id);

        public async void Remove(string id) =>
            await _users.DeleteOneAsync(user => user.Id == id);
    }
}
