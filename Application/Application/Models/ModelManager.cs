﻿using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
 using Application.Models;
using Application.SCMediator;
using Application.Services;
 using Application.Services.Implementations;
 using Microsoft.AspNetCore.Identity;

 namespace Application.Models
{
    public class ModelManager : Model
    {
        private ChatServiceImp chatServiceImp;
        private IAccountService accountService;
        private IChatroomService chatroomService;
        private ITopicsService topicService;
        public ModelManager()
        {
          this.chatServiceImp= new ChatServiceImp(this);
          this.accountService=new AccountsServiceImpl(this);
          this.topicService=new TopicsService(this);
          this.chatroomService=new ChatroomServiceImpl(this);

        }

        public async Task<List<Chatroom>> RequestChatrooms() {
            return await chatServiceImp.requestChatrooms();
        }

        public async Task<List<Account>> RequestUsers() { 
            return await chatServiceImp.requestUsers();
        }

        /*public async Task<List<Topic>> RequestTopics() {
            return await chatServiceImp.requestTopics();
        }*/

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

        public async Task DeleteChatroom(string ChatroomID) {
            await chatServiceImp.DeleteChatroom(ChatroomID);
        }
        public async Task DeleteTopic(string TopicID) {
            await chatServiceImp.DeleteTopic(TopicID);
        }

        public async Task RemoveUser(string userID) {
            await chatServiceImp.DeleteUser(userID);
        }

        public async Task Register(Account account) {
            await chatServiceImp.sendNewUser(account);
        }

        public void ProcessCredentials(string credentialsJson) {
            accountService.SetListOfAccounts(JsonSerializer.Deserialize<List<Account>>(credentialsJson));
        }
        public void ProcessChatrooms(string credentialsJson) {
            //chatroomService.Chatrooms = JsonSerializer.Deserialize<List<Chatroom>>(credentialsJson);
        }
        public void ProcessTopics(string credentialsJson) {
            //topicService.Topics = JsonSerializer.Deserialize<List<Topic>>(credentialsJson);
        }

        public async Task<Account> requestAccount(string username)
        {
            return await chatServiceImp.requestUser(username);
        }
    }
}
