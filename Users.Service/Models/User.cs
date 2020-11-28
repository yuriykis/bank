using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Users.Service.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }
    }
}
