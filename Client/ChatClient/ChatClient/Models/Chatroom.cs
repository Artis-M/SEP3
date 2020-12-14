using System.Collections.Generic;
using System.Threading.Tasks;

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

        public Chatroom(string id, string name, List<User> participants, List<Message> messages)
        {
            this._id = id;
            this.name = name;
            this.participants = participants;
            this.messages = messages;
        }

        public Chatroom(string id, string name, List<User> participants, List<Topic> topics)
        {
            this._id = id;
            this.name = name;
            this.participants = participants;
            this.topics = new List<Topic>();
        }

        public Chatroom(string id, string name, List<User> participants, List<Message> messages, List<Topic> topics)
        {
            this._id = id;
            this.name = name;
            this.participants = participants;
            this.messages = messages;
            this.topics = new List<Topic>();
        }

        public Chatroom(string id, string name, List<User> participants)
        {
            this._id = id;
            this.name = name;
            this.participants = participants;
        }

        public async Task removeUser(User user)
        {

        }

        public void addUser(User user)
        {
            
        }

        public void addTopic(Topic topic)
        {
            
        }

        public void removeTopic(Topic topic)
        {
            
        }

        public void addMessage(Message message)
        {
            messages.Add(message);
        }
    }
}