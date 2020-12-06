namespace Transactions.Service.Models
{
    public class TransactionUpdateModel
    {
        public string TransactionId { get; set; }
        
        public string SenderAccountId { get; set; }
        
        public string ReceiverAccountId { get; set; }
        
        public long Amount { get; set; }

        public string Status { get; set; }
        
        public string Info { get; set; }
    }
}