using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class MessageFragment
    {
            public string message { get; set; }
            
            public int Id { get; set; }
            public string username { get; set; }
            public string authorIdString { get; set; }
    }
}