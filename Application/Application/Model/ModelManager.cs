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

        public async Task<IList<Message>> getMessages()
        {
           return await connection.GetMessages();
        }
        


        public async Task SendReceived(Message message)
        {
            await connection.SaveMessage(message);
        }
    }
}