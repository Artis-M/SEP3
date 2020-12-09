using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Message
    {
        public string message { get; set; }
        public string messageId { get; set; }
        public string authorID { get; set; }
        public string username { get; set; }

        public Message(string message, string authorId, string messageId, string username)
        {
            this.message = message;
            this.authorID = authorId;
            this.messageId = messageId;
            this.username = username;
        }

        public Message()
        {
            
        }

        public string toString()
        {
            return "Message ID: " + messageId + ", Author ID: " + authorID + "Content of message: " + message+"Username: "+username;
        }
    }
}