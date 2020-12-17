using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services.Implementations
{
    /// <summary>
    /// Class used in the controller classes that is responsible for the passing of variables and the logic of them
    /// </summary>
    public class ChatroomServiceImpl : IChatroomService
    {
        public List<Chatroom> Chatrooms { get; set; }
        public Application.Models.Model model;

        /// <summary>
        /// Constructor initializing variables
        /// </summary>
        /// <param name="modelManager">model to be initialized</param>
        public ChatroomServiceImpl(Model modelManager)
        {
            this.Chatrooms = new List<Chatroom>();
            this.model = modelManager;
        }

        /// <summary>
        /// Getting a chat room by its ID
        /// </summary>
        /// <param name="ID">A chatroom's ID</param>
        /// <returns>Chat room with the given ID</returns>
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

        /// <summary>
        /// Requesting all chat rooms
        /// </summary>
        /// <returns>no return</returns>
        public async Task RequestChatrooms()
        {
            Chatrooms = await model.RequestChatrooms();
        }

        /// <summary>
        /// Getting all chat rooms the User is connected to
        /// </summary>
        /// <param name="id">ID of User</param>
        /// <returns>List of chatrooms the User is in</returns>
        public async Task<List<Chatroom>> GetChatroomByUserId(string id)
        {
            List<Chatroom> UsersChatrooms = await model.RequestChatroom(id);
            return UsersChatrooms;
        }

        /// <summary>
        /// Getting all chat rooms present in the system
        /// </summary>
        /// <returns>List of all chat rooms present</returns>
        public async Task<IList<Chatroom>> GetAllChatrooms()
        {
            await RequestChatrooms();
            return Chatrooms;
        }

        /// <summary>
        /// Adding a new chat room to the system
        /// </summary>
        /// <param name="chatroom">Chat room to be added</param>
        /// <returns>no return</returns>
        public async Task AddNewChatroom(Chatroom chatroom)
        {
            Chatrooms.Add(chatroom);
            await model.AddNewChatroom(chatroom);
        }

        /// <summary>
        /// Deleting a chat room
        /// </summary>
        /// <param name="ChatroomID">Id of chat room to be deleted</param>
        /// <returns>no return</returns>
        public async Task DeleteChatRoom(string ChatroomID)
        {
            await model.DeleteChatroom(ChatroomID);
        }

        /// <summary>
        /// Sending a message
        /// </summary>
        /// <param name="ChatroomID">ID of chat room the message should be sent to</param>
        /// <param name="message">Message to be sent</param>
        /// <returns>no return</returns>
        public async Task SendMessage(string ChatroomID, Message message)
        {
            await model.SendMessage(message, ChatroomID);
        }

        /// <summary>
        /// Adding a user to a chat room
        /// </summary>
        /// <param name="ChatRoomID">ID of chat room the User should be added to</param>
        /// <param name="userID">ID of user to be added</param>
        /// <returns>no return</returns>
        public async Task AddUser(string ChatRoomID, string userID)
        {
            await model.JoinChatroom(userID, ChatRoomID);
        }

        /// <summary>
        /// Removing user form a chatroom
        /// </summary>
        /// <param name="ChatRoomID">ID of chat room the User should be removed from</param>
        /// <param name="userID">ID of user to be removed</param>
        /// <returns>no return</returns>
        public async Task RemoveUser(string ChatRoomID, string userID)
        {
            await model.LeaveChatroom(userID, ChatRoomID);
        }

        /// <summary>
        /// Getting all the chat rooms that have the given topic
        /// </summary>
        /// <param name="topic">Topic that should be part of a chat room</param>
        /// <returns>List of chat rooms containing the given topic</returns>
        public async Task<List<Chatroom>> GetChatroomsByTopic(string topic)
        {
            return await model.GetChatroomsByTopic(topic);
        }

        /// <summary>
        /// Getting the private chat room of two friends
        /// </summary>
        /// <param name="user1">Friend 1</param>
        /// <param name="user2">Friend 2</param>
        /// <returns>Chat room of the two friends</returns>
        public async Task<Chatroom> GetPrivateChatroom(string user1, string user2)
        {
            return await model.GetPrivateChatroom(user1, user2);
        }
    }
}