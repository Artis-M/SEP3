﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Tier2.Http;
using Tier2.MediatorPlanB;

namespace Tier2.Model
{
    public class ModelManager : Model
    {
        
        private IHttpConnect connection;

        public ModelManager()
        {
            connection = new HttpConnect(); 

        }

       

        public async Task<IList<Chatroom>> getChatrooms()
        {
            //get Chatrooms
            //return await connection.GetMessages();
            return null;
        }

        public async Task sendMessage(Message message, int chatroomId)
        {//put in the selected chatRooms list
            await connection.SaveMessage(message);
        }
    }
}