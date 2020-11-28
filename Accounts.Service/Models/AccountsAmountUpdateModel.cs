using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Accounts.Service.Models
{
    public class AccountsAmountUpdateModel
    {
        public string SenderAccountId { get; set; }
        
        public string ReciverAccountId { get; set; }
        
        public long Amount { get; set; }
    }
}