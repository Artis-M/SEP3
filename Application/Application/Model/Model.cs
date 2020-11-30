﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tier2.Model
{
    public interface Model
    {
        public Task<IList<Chatroom>> getChatrooms();
        public Task sendMessage(Message message, int chatroomId);
    }
}