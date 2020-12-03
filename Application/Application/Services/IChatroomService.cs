using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Model;
using Tier2.Model;

namespace Application.Services
{
    public interface IChatroomService
    {
        public Task<Chatroom> GetChatroomByID(string ID);
        public Task<IList<Chatroom>> GetAllChatrooms();
        public Task AddNewChatroom(Chatroom chatroom);
        public Task DeleteChatRoom(string ChatroomID);
        public Task SendMessage(string ChatroomID, Message message);
        public Task AddUser(string ChatRoomID, User user);
        public Task RemoveUser(string ChatRoomID, User user);

    }
}