using System.Collections.Generic;
using System.Threading.Tasks;
using Tier2.Model;

namespace Services
{
    public interface IChatroomService
    {
        public Task<List<Chatroom>> GetUsersChatrooms(string userId);

        public Task CreateChatRoom(Chatroom chatroom);
    }
}