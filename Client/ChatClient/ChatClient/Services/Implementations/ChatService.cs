using System;
using System.Threading.Tasks;
using ChatClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Models;

namespace Services
{
    public class ChatService
    {
        string url = "https://localhost:5004/chathub";
        
        HubConnection _hubConnection = null;
        public Action<Message> newMessage;
        public Action<MessageFragment> newMessageFragment;
        public async Task ConnectToServer()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl(url).Build();
            await _hubConnection.StartAsync();

            _hubConnection.Closed += async (e) =>
            {
                Console.WriteLine("Exception:" + e.ToString());
                await _hubConnection.StartAsync();
            };
            _hubConnection.On<Message>("ReceiveChatRoomMessage", message =>
            {
                Console.WriteLine("What the fuck");
                Console.WriteLine($"Received message: {message.message}");
                newMessage?.Invoke(message);
            });
            _hubConnection.On<MessageFragment>("ReceiveChatRoomMessageFragment", messageFragment =>
            {
                Console.WriteLine(messageFragment.message);
                newMessageFragment?.Invoke(messageFragment);
            });
        }
        public async Task SendMessage(Message message, string activeChatRoomId)
        {
            Console.WriteLine("Sending message:" + message.ToString());
            await _hubConnection.SendAsync("SendMessage", message, activeChatRoomId);
        }
        public async Task SendMessageFragment(MessageFragment messageFragment, string activeChatRoomId)
        {
            await _hubConnection.SendAsync("SendMessageFragment", messageFragment, activeChatRoomId);
        }

        public async Task JoinChatRoom(string ChatRoomId)
        {
            await _hubConnection.SendAsync("JoinChatRoom", ChatRoomId);
        }
        public async Task LeaveChatRoom(string ChatRoomId)
        {
            await _hubConnection.SendAsync("LeaveChatRoom", ChatRoomId);
        }
    }
}