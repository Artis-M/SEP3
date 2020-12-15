using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Models
{
    public interface Model
    {
        public Task<List<Chatroom>> RequestChatrooms();
        public Task<List<Account>> RequestUsers();
        public Task SendMessage(Message message, string chatroomId);
        public Task AddNewChatroom(Chatroom chatroom);
        public Task LeaveChatroom(string userID, string chatroomID);
        public Task JoinChatroom(string userID, string chatroomID);
        public Task DeleteChatroom(string ChatroomID);
        public Task DeletePrivateChatroom(string userID, string friendID);
        public Task DeleteTopic(string topicID);
        public Task RemoveUser(string userID);
        public Task Register(Account account);
        public Task AddFriend(List<User> users);
        public Task<Account> RequestAccount(string username);
        public Task<Account> RequestAccountByID(string userID);
        public Task<List<Chatroom>> RequestChatroom(string userID);
        public Task EditAccount(Account account);
        public void RemoveTopicFromUser(string userId, string topic);
        public void AddTopicToUser(string userId, string topic);
        public Task<List<Chatroom>> GetChatroomsByTopic(string topic);
        public Task RemoveFriend(string userId, string friendId);
        public Task<Chatroom> GetPrivateChatroom(string user, string user1);
    }
}