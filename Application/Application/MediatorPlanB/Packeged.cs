using System.Collections.Generic;
using Tier2.Model;

namespace ChatClient.Services.Mediator
{
    public class Packeged
    {
        public string type { get; set; }
        public Message Message { get; set; }
        public IList<Message> messages { get; set; }
    }
}