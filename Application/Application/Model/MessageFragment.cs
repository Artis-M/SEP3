﻿using MongoDB.Bson;

namespace ChatClient.Models
{
    public class MessageFragment
    {
            public string message { get; set; }
            public int Id { get; set; }
            public ObjectId authorID { get; set; }
    }
}