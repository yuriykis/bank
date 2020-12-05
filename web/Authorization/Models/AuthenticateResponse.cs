using System;

namespace web.Authorization.Models
{
    public class AuthenticateResponse
    {
        public String Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        public string Token { get; set; }
        
    }
}