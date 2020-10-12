using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class FilmShowing
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("date")]
        public string date { get; set; }

        [BsonElement("roomId")]
        public string roomId { get; set; }

        [BsonElement("filmId")]
        public string filmId { get; set; }

        [BsonElement("numberOfSeatsInRoom")]
        public int numberOfSeatsInRoom { get; set; }
    }
}