namespace web.Models
{
    public class TransactionModel
    {
        public string SenderAccountId { get; set; }
        
        public string ReceiverAccountId { get; set; }
        
        public long Amount { get; set; }
        
        public string Status { get; set; }

        public string Info { get; set; }
    }
}