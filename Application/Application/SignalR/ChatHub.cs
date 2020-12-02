using System;
using System.Threading;
using System.Threading.Tasks;
using ChatClient.Models;
using Microsoft.AspNetCore.SignalR;
using Tier2.Model;

namespace WebApplication.SignalR
{
    public class ChatHub : Hub
    {
        public Task sendMessage(Message message)
        {
            Console.WriteLine("message sent?");
            return Clients.All.SendAsync("ReceiveMessage", message);
        }
        public Task sendMessageFragment(MessageFragment messageFragment)
        {
            Console.WriteLine("messageFragment sent?");
            return Clients.All.SendAsync("ReceiveMessageFragment", messageFragment);
        }
    }
}