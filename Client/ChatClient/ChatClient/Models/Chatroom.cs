using System.Collections.Generic;

namespace Models
{
    public class Chatroom
    {
        public string _id { get; set; }

        public string type { get; set; }
        public string owner { get; set; }
        public string name { get; set; }
        public List<Topic> topics { get; set; }
        public List<Message> messages { get; set; }
        public List<User> participants { get; set; }

        public Chatroom()
        {
            topics = new List<Topic>();
            messages = new List<Message>();
            participants = new List<User>();
        }
    }
}