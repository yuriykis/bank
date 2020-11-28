using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Transactions.Service.Models
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("sender_account_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SenderAccountId { get; set; }

        [BsonElement("receiver_account_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReciverAccountId { get; set; }

        [BsonElement("amount")]
        public long Amount { get; set; }
    }
}
