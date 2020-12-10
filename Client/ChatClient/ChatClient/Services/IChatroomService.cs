using System.Collections.Generic;
using System.Threading.Tasks;
using Tier2.Model;

namespace Services
{
    public interface IChatroomService
    {
        public Task<List<Chatroom>> GetUsersChatrooms(string userId);
        public Task CreateChatRoom(Chatroom chatroom);
        public Task JoinChatRoom(string chatroomId);
        public Task SetCurrentChatroom(string chatroomId);

        public Chatroom GetCurrentChatroom();
        
        public Task RemoveCurrentChatroom();
        public Task LeaveChatRoom(string userID, string chatroomID);

        public Task KickFromChatroom(string userID, string chatroomID);
    }
}