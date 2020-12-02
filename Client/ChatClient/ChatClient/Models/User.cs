using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatClient.Models
{
    public class User
    {
        public string username { get; set; }
        [BsonSerializer]
        public ObjectId userId { get; set; }
    }
}