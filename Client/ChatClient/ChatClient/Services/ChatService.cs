using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Models;

namespace Services
{
    public class ChatService
    {
        string url = "https://localhost:5004/chathub";
        
        HubConnection _hubConnection = null;
        public Action<Message> newMessage;
        public async Task ConnectToServer()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl(url).Build();
            await _hubConnection.StartAsync();

            _hubConnection.Closed += async (e) =>
            {
                Console.WriteLine(e);
                await _hubConnection.StartAsync();
            };
            _hubConnection.On<Message>("ReceiveMessage", message =>
            {
                Console.WriteLine(message.message);
                newMessage?.Invoke(message);
            });
        }
        public async Task SendMessage(Message message)
        {
            await _hubConnection.SendAsync("sendMessage", message);
        }
    }
}