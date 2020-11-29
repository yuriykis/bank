using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Users.Service.Authorization.Helpers;
using Users.Service.Authorization.Models;
using Users.Service.Models;

namespace Users.Service.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly AppSettings _appSettings;
        public UserService(IBankDatabaseSettings settings, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }
        
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await this.Get(model.Name, model.Password);
            
            if (user == null) return null;

            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IMongoCollection<User> GetAll()
        {
            return _users;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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

        public async Task<User> Get(string name, string password)
        {
            var response = await _users.FindAsync(user => user.Name == name && user.Password == password);
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
