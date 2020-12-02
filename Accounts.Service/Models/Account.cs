using System.ComponentModel.DataAnnotations.Schema;

namespace Accounts.Service.Models
{
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        
        public long Amount { get; set; }

        public string UserId { get; set; }
    }
}
