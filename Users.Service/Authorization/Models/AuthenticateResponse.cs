using System;
using Users.Service.Models;

namespace Users.Service.Authorization.Models
{
    public class AuthenticateResponse
    {
        public String Id { get; set; }
        
        public string Name { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Name = user.Name;
            Token = token;
        }
    }
}