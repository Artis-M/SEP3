﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Application.SCMediator;
using MongoDB.Bson;

namespace Tier2.Model
{
    public class ModelManager : Model
    {
        private ChatServiceImp chatServiceImp;
   

        public ModelManager(ChatServiceImp service)
        {
            chatServiceImp = service;

        }

       

        public async Task<IList<Chatroom>> getChatroom(string jSonThing)
        {
            await chatServiceImp.requestChatroom();
        }

        public async Task sendMessage(Message message, ObjectId chatroomId)
        {//put in the selected chatRooms list
            await chatServiceImp.sendMessage(message, chatroomId);
        }
    }
}