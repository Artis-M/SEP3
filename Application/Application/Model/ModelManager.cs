﻿using System.Collections.Generic;
 using System.Threading.Tasks;
 using Application.Model;
 

namespace Tier2.Model
{
    public class ModelManager : Model
    {
        
       

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

        public Task SendMessage(Message message, string chatroomId)
        {
            throw new System.NotImplementedException();
        }

        public Task Register(Account account)
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
    }
}