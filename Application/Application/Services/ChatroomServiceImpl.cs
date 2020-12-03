using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Model;
using Application.SCMediator;
using Tier2.Model;

namespace Application.Services
{
    public class ChatroomServiceImpl : IChatroomService
    {
        public List<Chatroom> Chatrooms { get; set; }

        public ChatroomServiceImpl()
        {
            this.Chatrooms = new List<Chatroom>();
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

        public async Task<IList<Chatroom>> GetAllChatrooms()
        {
            return Chatrooms;
        }

        public  async Task  AddNewChatroom(Chatroom chatroom)
        {
            Chatrooms.Add(chatroom);
        }

        public  async Task  DeleteChatRoom(string ChatroomID)
        {
            foreach (var VARIABLE in Chatrooms)
            {
                if (VARIABLE.id.Equals(ChatroomID))
                {
                    Chatrooms.Remove(VARIABLE);
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
                }
            }
        }

        public async Task RemoveUser(string ChatRoomID, User user)
        {
            foreach (var VARIABLE in Chatrooms)
            {
                if (VARIABLE.id.Equals(ChatRoomID))
                {
                    VARIABLE.removeUser(user);
                }
            }
        }
    }
}