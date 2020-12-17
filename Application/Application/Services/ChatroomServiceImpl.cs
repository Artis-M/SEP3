using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services.Implementations
{
    public class ChatroomServiceImpl : IChatroomService
    {
        public List<Chatroom> Chatrooms { get; set; }
        public Application.Models.Model model;

        public ChatroomServiceImpl(Model modelManager)
        {
            this.Chatrooms = new List<Chatroom>();
            this.model = modelManager;
        }

        public async Task<Chatroom> GetChatroomById(string ID)
        {
            await RequestChatrooms();
            foreach (var VARIABLE in Chatrooms)
            {
                if (VARIABLE._id.Equals(ID))
                {
                    return VARIABLE;
                }
            }

            return null;
        }

        public async Task RequestChatrooms()
        {
            Chatrooms = await model.RequestChatrooms();
        }

        public async Task<List<Chatroom>> GetChatroomByUserId(string id)
        {
            List<Chatroom> UsersChatrooms = await model.RequestChatroom(id);
            return UsersChatrooms;
        }

        public async Task<IList<Chatroom>> GetAllChatrooms()
        {
            await RequestChatrooms();
            return Chatrooms;
        }

        public async Task AddNewChatroom(Chatroom chatroom)
        {
            Chatrooms.Add(chatroom);
            await model.AddNewChatroom(chatroom);
        }

        public async Task DeleteChatRoom(string ChatroomID)
        {
            await model.DeleteChatroom(ChatroomID);
        }

        public async Task SendMessage(string ChatroomID, Message message)
        {
            await model.SendMessage(message, ChatroomID);
        }

        public async Task AddUser(string ChatRoomID, string userID)
        {
            await model.JoinChatroom(userID, ChatRoomID);
        }

        public async Task RemoveUser(string ChatRoomID, string userID)
        {
            await model.LeaveChatroom(userID, ChatRoomID);
        }

        public async Task<List<Chatroom>> GetChatroomsByTopic(string topic)
        {
            return await model.GetChatroomsByTopic(topic);
        }

        public async Task<Chatroom> GetPrivateChatroom(string user1, string user2)
        {
            return await model.GetPrivateChatroom(user1, user2);
        }
    }
}