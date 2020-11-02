using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("sender_account_id")]
        public string sender_account_id { get; set; }

        [BsonElement("reciver_account_id")]
        public string reciver_account_id { get; set; }

        [BsonElement("amount")]
        public long amount { get; set; }
    }
}
