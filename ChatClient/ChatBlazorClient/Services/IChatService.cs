using System.Collections.Generic;
using System.Threading.Tasks;
using ChatClient.Models;

namespace ChatClient.Services
{
    public interface IChatService
    {
        public Task sendMessage(Message message);
        public Task displayMessage(Message message);
        public  Task<List<ChatRoom>> getChatRooms();
        Task getMessageHistory();
    }
}