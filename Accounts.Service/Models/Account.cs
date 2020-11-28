using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Accounts.Service.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("amount")]
        public long Amount { get; set; }
        
        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
    }
}
