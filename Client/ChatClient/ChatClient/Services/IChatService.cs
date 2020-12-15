using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IChatService
    {
        public Action<Message> newMessage { get; set; }
        public Action<MessageFragment> newMessageFragment { get; set; }
        public Action<Chatroom> chatroomUpdate { get; set; }
        public Task ConnectToServer();
        public Task SendMessage(Message message, string activeChatRoomId);
        public Task SendMessageFragment(MessageFragment messageFragment, string activeChatRoomId);
        public Task JoinChatRoom(string ChatRoomId);
        public Task LeaveChatRoom(string ChatRoomId);
        public Task DisconnectFromHub();
        public Task UpdateChatRooms(List<Chatroom> chatrooms, User userToRemove);
    }
}