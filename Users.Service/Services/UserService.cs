using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Users.Service.Authorization.Helpers;
using Users.Service.Authorization.Models;
using Users.Service.Models;
using Users.Service.Persistence;

namespace Users.Service.Services
{
    public class UserService
    {

        private readonly AppSettings _appSettings;
        private readonly PrimaryContext _context;
        public UserService(IOptions<AppSettings> appSettings, PrimaryContext context)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await this.Get(model.Username, model.Password);
            
            if (user == null) return null;

            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
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
        
        public Task<List<User>> Get()
        {
            return _context.Users.ToListAsync();
        }
        
        public async Task<User> Get(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> Get(string username, string password)
        {
            try
            {
                var response = await _context.Users.SingleOrDefaultAsync(
                    user => user.Username == username && user.Password == password
                );
                return response;
            }
            catch (Exception e)
            {
                return new User();
            }
        }

        public async Task<User> Create(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task Update(string id, User userIn)
        {
            _context.Users.Update(userIn);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(User userIn)
        {
            _context.Users.Remove(userIn);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(string id)
        {
            var user = await  _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
