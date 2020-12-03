using System.Collections.Generic;
using Application.Model;
using MongoDB.Bson;

namespace Tier2.Model
{
    public class Chatroom
    {
        public string id { get; set; }
        public string name { get; set; }
        public TopicList topics { get; set; }
        public MessageList messages { get; set; }
        public UserList participants { get; set; }

        public Chatroom(string id, string name, UserList participants, MessageList messages)
        {
            this.id = id;
            this.name = name;
            this.participants = new UserList();
            this.messages = new MessageList();
        }

        public Chatroom(string id, string name, UserList participants, TopicList topics)
        {
            this.id = id;
            this.name = name;
            this.participants = new UserList();
            this.topics = new TopicList();
        }

        public Chatroom(string id, string name, UserList participants, MessageList messages, TopicList topics)
        {
            this.id = id;
            this.name = name;
            this.participants = new UserList();
            this.messages = new MessageList();
            this.topics = new TopicList();
        }

        public Chatroom(string id, string name, UserList participants)
        {
            this.id = id;
            this.name = name;
            this.participants = new UserList();
        }

        public void removeUser(User user)
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
            messages.addMessage(message);
        }
    }
}