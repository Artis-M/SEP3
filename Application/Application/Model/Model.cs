﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tier2.Model
{
    public interface Model
    {
        public Task<IList<Message>> getMessages();
        public Task SendReceived(Message message);
    }
}