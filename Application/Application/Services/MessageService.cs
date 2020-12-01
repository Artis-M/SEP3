using System.Collections.Generic;
using System.Threading.Tasks;
using Tier2.Model;

namespace Application.Services
{
    public class MessageService : IMessageService
    {
        private IList<Chatroom> chatrooms;
        private ModelManager Model = new ModelManager();

        public MessageService()
        {
            this.chatrooms = new List<Chatroom>();
        }


        public async Task<IList<Chatroom>> getChatrooms()
        {
            this.chatrooms = await Model.getChatrooms();
            return chatrooms;
        }

        public async Task<Message> sendMessage(Message message, int chatRoomId)
        {
            await Model.sendMessage(message, chatRoomId);
            return message;
        }
    }
}