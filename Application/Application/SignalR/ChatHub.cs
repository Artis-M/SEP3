using System;
using System.Threading;
using System.Threading.Tasks;
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

        /*public void sendLoop()
        {
            while (true)
            {
                Thread.Sleep(2000);
                Message msg1 = new Message
                {
                    text = "dasdasdas"
                };
                sendMessage(msg1);
            }
        }*/
    }
}