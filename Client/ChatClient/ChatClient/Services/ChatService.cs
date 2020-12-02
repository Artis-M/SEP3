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
                Console.WriteLine(e);
                await _hubConnection.StartAsync();
            };
            _hubConnection.On<Message>("ReceiveMessage", message =>
            {
                Console.WriteLine(message.message);
                newMessage?.Invoke(message);
            });
            _hubConnection.On<MessageFragment>("ReceiveMessageFragment", messageFragment =>
            {
                Console.WriteLine(messageFragment.message);
                newMessageFragment?.Invoke(messageFragment);
            });
        }
        public async Task SendMessage(Message message)
        {
            await _hubConnection.SendAsync("sendMessage", message);
        }
        public async Task SendMessageFragment(MessageFragment messageFragment)
        {
            await _hubConnection.SendAsync("sendMessageFragment", messageFragment);
        }
    }
}