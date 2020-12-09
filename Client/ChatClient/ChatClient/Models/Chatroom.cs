﻿﻿using System.Collections.Generic;
  using System.Threading.Tasks;
  using Application.Models;
  using Models;
  using MongoDB.Bson;

 namespace Tier2.Model
{
    public class Chatroom
    {
        public string _id { get; set; }
        public string name { get; set; }
        public List<Topic> topics { get; set; }
        public List<Message> messages { get; set; }
        public List<User> participants { get; set; }

        public Chatroom()
        {
            participants = new List<User>();
            messages = new List<Message>();
            topics = new List<Topic>();
        }

        public Chatroom(string id, string name, List<User> participants, List<Message> messages)
        {
            this._id = id;
            this.name = name;
            this.participants = participants;
            this.messages = messages;
        }

        public Chatroom(string id, string name, List<User> participants, TopicList topics)
        {
            this._id = id;
            this.name = name;
            this.participants = participants;
            this.topics = new List<Topic>();
        }

        public Chatroom(string id, string name, List<User> participants, List<Message> messages, TopicList topics)
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