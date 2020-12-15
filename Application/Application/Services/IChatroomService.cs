using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services
{
    public interface IChatroomService
    {
        public Task<Chatroom> GetChatroomById(string ID);
        public Task<List<Chatroom>> GetChatroomByUserId(string id);
        public Task<IList<Chatroom>> GetAllChatrooms();
        public Task AddNewChatroom(Chatroom chatroom);
        public Task DeleteChatRoom(string ChatroomID);
        public Task SendMessage(string ChatroomID, Message message);
        public Task AddUser(string ChatRoomID, string userID);
        public Task RemoveUser(string ChatRoomID, string userID);
        public Task<List<Chatroom>> GetChatroomsByTopic(string topic);
        public Task<Chatroom> GetPrivateChatroom(string user1, string user2);
    }
}