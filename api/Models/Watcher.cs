using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Watcher
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("seatNumber")]
        public int seatNumber { get; set; }

        [BsonElement("userId")]
        public string userId { get; set; }

        [BsonElement("filmShowingId")]
        public string filmShowingId { get; set; }
    }
}
