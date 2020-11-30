using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tier2.Model;

namespace Application.Services
{
    public interface IMessageService
    {
        Task<IList<Chatroom>> getChatrooms();
        Task<Message> sendMessage(Message message,int chatRoomId);
    }
}