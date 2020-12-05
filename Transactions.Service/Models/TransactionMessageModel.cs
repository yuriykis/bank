namespace Transactions.Service.Models
{
    public class TransactionMessageModel
    {
        public string SenderAccountId { get; set; }
        
        public string ReceiverAccountId { get; set; }
        
        public long Amount { get; set; }

        public string Message { get; set; }
    }
}