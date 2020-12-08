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
        private IChatroomService _chatroomServiceImplementation;

        public ChatroomServiceImpl(Model modelManager)
        {
            this.Chatrooms = new List<Chatroom>();
            this.model = modelManager;
        }

        public async Task<Chatroom> GetChatroomByID(string ID)
        {
            foreach (var VARIABLE in Chatrooms)
            {
                if (VARIABLE.id.Equals(ID))
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
            List<Chatroom> ChatroomsForUser = await model.RequestChatrooms();
            List<Chatroom> UsersChatrooms = new List<Chatroom>();
            foreach (var item in ChatroomsForUser)
            {
               User user = item.participants.Users.First(user => user._id.Equals(id));
               if (user != null)
               {
                   UsersChatrooms.Add(item);
               }
            }
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
                if (VARIABLE.id.Equals(ChatroomID))
                {
                    Chatrooms.Remove(VARIABLE);
                    await model.DeleteChatroom(ChatroomID);
                }
            }

           
        }

        public async Task SendMessage(string ChatroomID, Message message)
        {
            foreach (var VARIABLE in Chatrooms)
            {
                if (VARIABLE.id.Equals(ChatroomID))
                {
                    VARIABLE.addMessage(message);
                    await model.SendMessage(message, ChatroomID);
                }
            }

          
        }

        public async Task AddUser(string ChatRoomID, User user)
        {
            foreach (var VARIABLE in Chatrooms)
            {
                if (VARIABLE.id.Equals(ChatRoomID))
                {
                    VARIABLE.addUser(user);
                    await model.UpdateChatroom(VARIABLE);
                }
            }
           
        }

        public async Task RemoveUser(string ChatRoomID, User user)
        {
            foreach (var VARIABLE in Chatrooms)
            {
                if (VARIABLE.id.Equals(ChatRoomID))
                {
                    await VARIABLE.removeUser(user);
                    await model.UpdateChatroom(VARIABLE);
                }
            }
        }
    }
}