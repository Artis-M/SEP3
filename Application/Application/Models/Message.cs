using MongoDB.Bson;

namespace Application.Models
{
    public class Message
    {
        public string message { get; set; }
        public string Id { get; set; }
        public string authorID { get; set; }

        public Message(string message, string authorId, string id)
        {
            this.message = message;
            this.authorID = authorId;
            this.Id = id;
        }

        public string toString()
        {
            return "Message ID: " + Id + ", Author ID: " + authorID + "Content of message: " + message;
        }
    }
}