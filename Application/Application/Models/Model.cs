﻿using System.Collections.Generic;
using System.Threading.Tasks;
 using Application.Models;
 using Application.Services;
 using MongoDB.Bson;

namespace Application.Models
{
    public interface Model
    {
        public Task<List<Chatroom>> RequestChatrooms();
        public Task<List<Account>> RequestUsers();
        //public Task<List<Topic>> RequestTopics();
        public Task SendMessage(Message message, string chatroomId);
        public Task AddNewChatroom(Chatroom chatroom);
        public Task UpdateChatroom(Chatroom chatroom);
        public Task LeaveChatroom(string userID, string chatroomID);
        public Task JoinChatroom(string userID, string chatroomID);
        public Task DeleteChatroom(string ChatroomID);
        public Task DeletePrivateChatroom(string userID, string friendID);
        public Task DeleteTopic(string topicID);
        public Task RemoveUser(string userID);
        public Task Register(Account account);
        public Task AddFriend(List<User> users);
        public void ProcessCredentials(string credentialsJson);
        public void ProcessChatrooms(string credentialsJson);
        public void ProcessTopics(string credentialsJson);
        public Task<Account> requestAccount(string username);
        public Task<Account> requestAccountByID(string userID);
        public Task<List<Chatroom>> requestChatroom(string userID);
        public Task EditAccount(Account account);
        public void removeTopicFromUser(string userId, string topic);
        public void addTopicToUser(string userId, string topic);

        public Task<List<Chatroom>> getChatroomsByTopic(string topic);
        public Task removeFriend(string userId, string friendId);
        public Task<Chatroom> getPrivateChatroom(string user, string user1);
        
    }
}