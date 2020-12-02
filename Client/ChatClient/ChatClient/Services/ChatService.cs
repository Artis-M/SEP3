using System;
using System.Text.Json;
using System.Threading.Tasks;
using ChatClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

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
                Console.WriteLine(e);
                await _hubConnection.StartAsync();
            };
            _hubConnection.On<string>("ReceiveMessage", messageString =>
            {
                Message message = JsonSerializer.Deserialize<Message>(messageString);
                Console.WriteLine(message.message);
                newMessage?.Invoke(message);
            });
            _hubConnection.On<string>("ReceiveMessageFragment", messageFragmentString =>
            {
                MessageFragment messageFragment = JsonSerializer.Deserialize<MessageFragment>(messageFragmentString);
                Console.WriteLine(messageFragment.message);
                newMessageFragment?.Invoke(messageFragment);
            });
        }
        public async Task SendMessage(Message message)
        {
            Console.WriteLine(JsonSerializer.Serialize(message));
            await _hubConnection.SendAsync("sendMessage", JsonSerializer.Serialize(message));
        }
        public async Task SendMessageFragment(MessageFragment messageFragment)
        {
            Console.WriteLine(JsonSerializer.Serialize(messageFragment));
            await _hubConnection.SendAsync("sendMessageFragment", JsonSerializer.Serialize(messageFragment));
        }
    }
}