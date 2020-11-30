using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ChatClient.Models;
using ChatClient.Services.Mediator;

namespace ChatClient.Services
{
    public class ChatServiceImp : IChatService
    {
        private TcpClient Client;
        private NetworkStream stream;
        private ClientHandler clientHandler;
        public Action<Message> newMessage;
        public Action<IList<Message>> messageHistory;

        public ChatServiceImp()
        {
            connectToServer("localhost", 25565);
        }

        public async Task connectToServer(string ip, int port)
        {
            try
            {
                Client = new TcpClient(ip, port);
                clientHandler = new ClientHandler(stream, Client, this);
            }
            catch (Exception e)
            {
                Console.WriteLine("Connecting to server. Retrying.");
                connectToServer("localhost", 1337);
            }

            stream = Client.GetStream();
        }

        public async Task sendMessage(Message message)
        {
            Packeged packeged = new Packeged {type = "message", Message = message};

            string packageserial = JsonSerializer.Serialize(packeged);
            stream = Client.GetStream();
            byte[] dataToServer = Encoding.ASCII.GetBytes(packageserial);
            stream.Write(dataToServer, 0, dataToServer.Length);
        }

        public async Task displayMessage(Message message)
        {
            newMessage?.Invoke(message);
        }

        public async Task<IList<Message>> displayMessageHistory(IList<Message> message)
        {
            messageHistory.Invoke(message);
            return message;
        }

        public async Task<List<ChatRoom>> getChatRooms()
        {
            throw new System.NotImplementedException();
        }

        public async Task getMessageHistory()
        {
            Packeged packeged = new Packeged
            {
                type = "request",
                messages = null
            };

            string request = JsonSerializer.Serialize(packeged);
            stream = Client.GetStream();
            byte[] dataToServer = Encoding.ASCII.GetBytes(request);
            stream.Write(dataToServer, 0, dataToServer.Length);
        }
    }
}