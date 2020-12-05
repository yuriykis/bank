using System;
using System.ComponentModel.DataAnnotations;

namespace Users.Service.Authorization.Models
{
    public class AuthenticateRequest
    {

        [Required] 
        
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}