using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Application.SCMediator;
using Application.Services;
using Application.Services.Implementations;

namespace Application.Models
{
    public class ModelManager : Model
    {
        private ChatServiceImp chatServiceImp;
        private IAccountService accountService;
        private IChatroomService chatroomService;
        private ITopicsService topicService;

        public ModelManager()
        {
            this.chatServiceImp = new ChatServiceImp(this);
            this.accountService = new AccountsServiceImpl(this);
            this.topicService = new TopicsService(this);
            this.chatroomService = new ChatroomServiceImpl(this);
        }

        public async Task<List<Chatroom>> RequestChatrooms()
        {
            return await chatServiceImp.RequestChatrooms();
        }

        public async Task<List<Account>> RequestUsers()
        {
            return await chatServiceImp.RequestUsers();
        }
        public async Task SendMessage(Message message, string chatroomId)
        {
            await chatServiceImp.SendMessage(message, chatroomId);
        }

        public async Task AddNewChatroom(Chatroom chatroom)
        {
            await chatServiceImp.SendNewChatroom(chatroom);
        }

        public async Task UpdateChatroom(Chatroom chatroom)
        {
            await chatServiceImp.SendChatroomUpdate(chatroom);
        }

        public async Task LeaveChatroom(string userID, string chatroomID)
        {
            await chatServiceImp.LeaveChatroom(chatroomID, userID);
        }

        public async Task JoinChatroom(string userID, string chatroomID)
        {
            await chatServiceImp.JoinChatroom(chatroomID, userID);
        }

        public async Task DeleteChatroom(string ChatroomID)
        {
            await chatServiceImp.DeleteChatroom(ChatroomID);
        }

        public async Task DeletePrivateChatroom(string userID, string friendID)
        {
            await chatServiceImp.DeletePrivateChatroom(userID, friendID);
        }

        public async Task DeleteTopic(string TopicID)
        {
            await chatServiceImp.DeleteTopic(TopicID);
        }

        public async Task RemoveUser(string userID)
        {
            await chatServiceImp.DeleteUser(userID);
        }

        public async Task Register(Account account)
        {
            await chatServiceImp.SendNewUser(account);
        }

        public async Task AddFriend(List<User> users)
        {
            await chatServiceImp.AddFriend(users);
        }

        public void ProcessCredentials(string credentialsJson)
        {
            accountService.SetListOfAccounts(JsonSerializer.Deserialize<List<Account>>(credentialsJson));
        }
        public async Task<Account> RequestAccount(string username)
        {
            return await chatServiceImp.RequestUser(username);
        }

        public async Task<Account> RequestAccountByID(string userID)
        {
            return await chatServiceImp.RequestUserById(userID);
        }

        public async Task<List<Chatroom>> RequestChatroom(string userID)
        {
            return await chatServiceImp.RequestUsersChatroom(userID);
        }

        public async Task EditAccount(Account account)
        {
            await chatServiceImp.SendUserUpdate(account);
        }

        public async void RemoveTopicFromUser(string userId, string topic)
        {
           await chatServiceImp.RemoveTopicfromUser(userId, topic);
        }

        public async void AddTopicToUser(string userId, string topic)
        {
            await chatServiceImp.AddTopicToUser(userId, topic);
        }

        public async Task<List<Chatroom>> GetChatroomsByTopic(string topic)
        {
            return await chatServiceImp.RequestChatroomsByTopic(topic);
        }

        public async Task RemoveFriend(string userId, string friendId)
        {
            await chatServiceImp.RemoveFriend(userId, friendId);
        }

        public async Task<Chatroom> GetPrivateChatroom(string user, string user1)
        {
            return await chatServiceImp.GetPrivateChatroom(user, user1);
        }
    }
}