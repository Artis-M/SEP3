using MongoDB.Bson;

namespace ChatClient.Models
{
    public class User
    {
        public string username { get; set; }
        public ObjectId userId { get; set; }
    }
}