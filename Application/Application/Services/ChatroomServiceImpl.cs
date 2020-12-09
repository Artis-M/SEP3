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

        public async Task<Chatroom> GetChatroomByID(string ID)
        {
            foreach (var VARIABLE in Chatrooms)
            {
                if (VARIABLE._id.Equals(ID))
                {
                    return VARIABLE;
                }
            }

            return null;
        }

        public async Task requestChatrooms()
        {
            Chatrooms = await model.RequestChatrooms();
        }

        public async Task<List<Chatroom>> GetChatroomByUserID(string id)
        {
            
            List<Chatroom> UsersChatrooms = await model.requestChatroom(id);
            return UsersChatrooms;
        }

        public async Task<IList<Chatroom>> GetAllChatrooms()
        {
            await requestChatrooms();
            return Chatrooms;
        }

        public async Task AddNewChatroom(Chatroom chatroom)
        {
            Chatrooms.Add(chatroom);
            await model.AddNewChatroom(chatroom);
        }

        public async Task DeleteChatRoom(string ChatroomID)
        {
            foreach (var VARIABLE in Chatrooms)
            {
                if (VARIABLE._id.Equals(ChatroomID))
                {
                    Chatrooms.Remove(VARIABLE);
                    await model.DeleteChatroom(ChatroomID);
                }
            }

           
        }

        public async Task SendMessage(string ChatroomID, Message message)
        {
            await model.SendMessage(message, ChatroomID);
        }

        public async Task AddUser(string ChatRoomID,  string userID)
        {
           
                    // here the chatroom should update so that the user is inside, the one that is sent to all users somehow
                    await model.JoinChatroom(userID,ChatRoomID);
                

    }

        public async Task RemoveUser(string ChatRoomID,  string userID)
        {
            // the chatroom should update so that the user is not inside it anymore
                    await model.LeaveChatroom(userID,ChatRoomID);
                
        }
    }
}