﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Tier2.Model;

namespace Tier2.Http
{
    public interface IHttpConnect
    {
        Task SaveMessage(Message message);
        Task<List<Message>> GetMessages();
    }
}