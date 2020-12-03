﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Tier2.Model
{
    public interface Model
    {
        public Task<IList<Chatroom>> getChatroom(string json);
        public Task sendMessage(Message message, ObjectId chatroomId);
        public Task ProcessCredentials(ObjectId user, string jsonCredentials);
    }
}