﻿using System.Collections.Generic;

namespace Tier2.Model
{
    public class Chatroom
    {
        public IList<Message> Messages;
        public int ID { get; set; }
        public string Name { get; set; }

        public Chatroom()
        {
            Messages = new List<Message>();
        }

        // public Message getMessage(int id)
        // {
        //     foreach (var message in Messages)
        //     {
        //         if (message.id == id)
        //         {
        //             return message;
        //         }
        //     }
        //
        //     return null;
        // }
    }
}