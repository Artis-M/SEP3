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

        public Task Register(Account account)
        {
            chatServiceImp.sendNewUser(account);
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
    }
}
