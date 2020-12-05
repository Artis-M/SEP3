﻿using System.Collections.Generic;
using System.Threading.Tasks;
 using Application.Models;
 using MongoDB.Bson;

namespace Application.Models
{
    public interface Model
    {
        public Task RequestChatrooms();
        public Task<List<Account>> RequestUsers();
        public Task RequestTopics();
        public Task SendMessage(Message message, string chatroomId);
        public Task AddNewChatroom(Chatroom chatroom);
        public Task UpdateChatroom(Chatroom chatroom);
        public Task DeleteChatroom(string ChatroomID);
        public Task RemoveUser(string userID);
        public Task Register(Account account);
        public void ProcessCredentials(string credentialsJson);
        public void ProcessChatrooms(string credentialsJson);
        public void ProcessTopics(string credentialsJson);
    }
}