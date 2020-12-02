using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tier2.Model
{
    public class Message
    {
        public string message { get; set; }
        public int Id { get; set; }
        public ObjectId authorID { get; set; }
        public string authorIdString { get; set; }
    }
}