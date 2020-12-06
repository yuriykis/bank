using System.ComponentModel.DataAnnotations.Schema;

namespace Transactions.Service.Models
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        
        public string SenderAccountId { get; set; }
        
        public string ReceiverAccountId { get; set; }
        
        public long Amount { get; set; }

        
        public string Status { get; set; }

        public string Info { get; set; }
    }
}
