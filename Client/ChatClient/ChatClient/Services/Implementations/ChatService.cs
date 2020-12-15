using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Models;

namespace Services
{
    public class ChatService : IChatService
    {
        string url = "https://localhost:5004/chathub";
        private string uri = "https://localhost:5004/chatrooms/";

        HubConnection _hubConnection = null;
        public Action<Message> newMessage { get; set; }
        public Action<MessageFragment> newMessageFragment { get; set; }
        public Action<Chatroom> chatroomUpdate { get; set; }

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
                Console.WriteLine($"Received message: {message.message}");
                newMessage?.Invoke(message);
            });
            _hubConnection.On<MessageFragment>("ReceiveChatRoomMessageFragment",
                messageFragment => { newMessageFragment?.Invoke(messageFragment); });
            _hubConnection.On<Chatroom>("ReceiveChatroomUpdate", chatroom => { chatroomUpdate.Invoke(chatroom); });
        }

        public async Task SendMessage(Message message, string activeChatRoomId)
        {
            await _hubConnection.SendAsync("SendMessage", message, activeChatRoomId);

            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            StringContent content =
                new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            string request = $"sendMessage/{activeChatRoomId}";
            Console.Out.WriteLine(content);
            await http.PostAsync(request, content);
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

        public async Task DisconnectFromHub()
        {
            await _hubConnection.StopAsync();
        }

        public async Task UpdateChatRooms(List<Chatroom> chatrooms, User userToRemove)
        {
            foreach (var item in chatrooms)
            {
                item.participants.Remove(item.participants.First((user => user._id.Equals(userToRemove._id))));
                await _hubConnection.SendAsync("UpdateChatroom", item);
            }
        }
    }
}