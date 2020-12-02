using System;
using System.ComponentModel.DataAnnotations;

namespace Users.Service.Authorization.Models
{
    public class AuthenticateRequest
    {

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}