﻿﻿using System.Collections.Generic;
 using System.Threading.Tasks;
 using Tier2.MediatorPlanB;

namespace Tier2.Model
{
    public class ModelManager : Model
    {
        
       

        public ModelManager()
        {
          

        }

       

        public async Task<IList<Chatroom>> getChatrooms()
        {
            //get Chatrooms
            //return await connection.GetMessages();
            return null;
        }

        public async Task sendMessage(Message message, int chatroomId)
        {//put in the selected chatRooms list
            
        }
    }
}