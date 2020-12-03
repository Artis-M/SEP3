﻿using System.Collections.Generic;
using System.Text.Json;
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

       

        public async Task<IList<Chatroom>> getChatroom(ObjectId chatroomID)
        {
            await chatServiceImp.requestChatroom(chatroomID);
        }

        public async Task sendMessage(Message message, ObjectId chatroomId)
        {//put in the selected chatRooms list
            await chatServiceImp.sendMessage(message, chatroomId);
        }

        // ------------------- //
        //     processing      //
        // ------------------- //

        public async Task ProcessChatroom() {

        }

        public async Task ProcessCredentials(string jsonCredentials) {
            LoginBox credentials = JsonSerializer.Deserialize<LoginBox>(jsonCredentials);

        }


    }
}