﻿using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
 using Application.Model;
using Application.SCMediator;
using Application.Services;

namespace Tier2.Model
{
    public class ModelManager : Model
    {
        ChatServiceImp chatServiceImp;
        private AccountsServiceImpl accountService;
        private ChatroomServiceImpl chatroomService;
        private TopicsService topicService;
        public ModelManager()
        {
          

        }

        public Task RequestChatrooms()
        {
            throw new System.NotImplementedException();
        }

        public Task RequestUsers()
        {
            throw new System.NotImplementedException();
        }

        public Task RequestTopics()
        {
            throw new System.NotImplementedException();
        }

        public async Task SendMessage(Message message, string chatroomId)
        {
            chatServiceImp.sendMessage(message, chatroomId);
        }

        public async Task Register(Account account)
        {
            chatServiceImp.sendNewUser(account);
        }

        public Task Login(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Chatroom>> RecieveChatrooms()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<User>> RecieveUsers()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Topic>> RecieveTopics()
        {
            throw new System.NotImplementedException();
        }
        public async Task ProcessCredentials(string credentialsJson) {
            accountService.Accounts = JsonSerializer.Deserialize<List<Account>>(credentialsJson);
        }
        public async Task ProcessChatrooms(string credentialsJson) {
            chatroomService.Chatrooms = JsonSerializer.Deserialize<List<Chatroom>>(credentialsJson);
        }
        public async Task ProcessTopics(string credentialsJson) {
            topicService.Topics = JsonSerializer.Deserialize<List<Topic>>(credentialsJson);
        }

    }
}
