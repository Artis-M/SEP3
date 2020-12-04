﻿using System.Collections.Generic;
using System.Threading.Tasks;
 using Application.Model;
 using MongoDB.Bson;

namespace Tier2.Model
{
    public interface Model
    {
        public Task RequestChatrooms();
        public Task RequestUsers();
        public Task RequestTopics();
        public Task SendMessage(Message message, string chatroomId);
        public Task Register(Account account);
        public Task Login(string username, string password);
        public Task<List<Chatroom>> RecieveChatrooms();
        public Task<List<User>> RecieveUsers();
        public Task<List<Topic>> RecieveTopics();
        public Task ProcessCredentials(string credentialsJson);
        public Task ProcessChatrooms(string credentialsJson);
        public Task ProcessTopics(string credentialsJson);
    }
}