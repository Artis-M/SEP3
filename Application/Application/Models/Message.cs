using MongoDB.Bson;

namespace Application.Models
{
    public class Message
    {
        public string message { get; set; }
        public string messageId { get; set; }
        public string authorID { get; set; }

        public Message(string message, string authorId, string messageId)
        {
            this.message = message;
            this.authorID = authorId;
            this.messageId = messageId;
        }

        public string toString()
        {
            return "Message ID: " + messageId + ", Author ID: " + authorID + "Content of message: " + message;
        }
    }
}