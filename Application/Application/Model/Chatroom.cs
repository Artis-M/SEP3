﻿using System.Collections.Generic;

namespace Tier2.Model
{
    public class Chatroom
    {
        private IList<Message> Messages;
        private int ID { get; set; }

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