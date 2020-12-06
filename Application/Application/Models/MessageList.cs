using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Models
{
    public class MessageList
    {
        public List<Message> Messages { get; set; }

        public MessageList()
        {
            this.Messages = new List<Message>();
        }

        public MessageList(List<Message> messages)
        {
            this.Messages = messages;
        }

        public void addMessage(Message message)
        {
            Messages.Add(message);
        }

        public void removeMessage(Message message)
        {
            //we can do with message ID as well
            Messages.Remove(message);
        }
    }
}