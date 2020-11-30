using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ChatClient.Models;

namespace ChatClient.Services.Mediator
{
    public class ClientHandler
    {
        public TcpClient Client;
        public NetworkStream stream;
        public ChatServiceImp ServiceImp;


        public ClientHandler(NetworkStream stream, TcpClient client, ChatServiceImp service)
        {
            Console.WriteLine("Client Handler Starting!");
            Client = client;
            stream = stream;
            ServiceImp = service;
            Thread t1 = new Thread(MessageHandler);
            t1.Start();
        }

        public void MessageHandler()
        {
            while (true)
            {
                stream = Client.GetStream();
                byte[] dataFromServer = new byte[1024];
                int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
                string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
                Console.WriteLine(response);
                Packeged packeged = JsonSerializer.Deserialize<Packeged>(response);
                Console.WriteLine("Packaged "+packeged.type+"  =>>>  "+packeged.messages);
                if (packeged.type.Equals("list"))
                {
                    IList<Message> messages = (packeged.messages);
                    ServiceImp.displayMessageHistory(messages);
                }
                else if (packeged.type.Equals("message"))
                {
                    Message message = (packeged.Message);
                    ServiceImp.displayMessage(message);
                }
            }
        }
    }
}