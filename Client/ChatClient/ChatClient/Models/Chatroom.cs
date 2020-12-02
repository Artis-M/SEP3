﻿﻿using System.Collections.Generic;
  using Models;
  using MongoDB.Bson;

 namespace Tier2.Model
{
    public class Chatroom
    {
        public IList<Message> Messages;
        public ObjectId ID { get; set; }
        public string Name { get; set; }

        public Chatroom()
        {
            Messages = new List<Message>();
        }
    }
}