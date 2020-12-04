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
        private ChatServiceImp chatServiceImp;
        private AccountsServiceImpl accountService;
        private ChatroomServiceImpl chatroomService;
        private TopicsService topicService;
        public ModelManager()
        {
          

        }

        public async Task RequestChatrooms()
        {
            await chatServiceImp.requestChatrooms();
        }

        public async Task RequestUsers()
        {
            await chatServiceImp.requestUserCredentials();
        }

        public async Task RequestTopics()
        {
            await chatServiceImp.requestTopics();
        }

        public async Task SendMessage(Message message, string chatroomId)
        {
            await chatServiceImp.sendMessage(message, chatroomId);
        }

        public async Task AddNewChatroom(Chatroom chatroom)
        {
            await chatServiceImp.sendNewChatroom(chatroom);
        }

        public async Task UpdateChatroom(Chatroom chatroom)
        {
            await chatServiceImp.sendChatroomUpdate(chatroom);
        }

        public async Task DeleteChatroom(string ChatroomID)
        {
            //
        }

        public async Task Register(Account account)
        {
            await chatServiceImp.sendNewUser(account);
        }

        public void ProcessCredentials(string credentialsJson) {
            accountService.Accounts = JsonSerializer.Deserialize<List<Account>>(credentialsJson);
        }
        public void ProcessChatrooms(string credentialsJson) {
            chatroomService.Chatrooms = JsonSerializer.Deserialize<List<Chatroom>>(credentialsJson);
        }
        public void ProcessTopics(string credentialsJson) {
            topicService.Topics = JsonSerializer.Deserialize<List<Topic>>(credentialsJson);
        }

    }
}
