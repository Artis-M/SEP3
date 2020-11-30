﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using ChatClient.Services.Mediator;
using Tier2.Model;

namespace Tier2.MediatorPlanB
{
    public class ClientHandler
    {
        public TcpClient Client;
        public ClientConnector ClientConnector;
        public Model.Model model;

        public ClientHandler(TcpClient client, ClientConnector clientConnector)
        {
            this.model = new ModelManager();
            ClientConnector = clientConnector;
            Client = client;
            Thread t1 = new Thread((() => HandleClient(Client)));
            t1.Start();
        }

        public async void HandleClient(TcpClient client)
        {
            // while (true)
            // {
            //     try
            //     {
            //         NetworkStream stream = client.GetStream();
            //         //read
            //         byte[] dataFromClient = new byte[1024];
            //
            //         int bytesRead = stream.Read(dataFromClient, 0, dataFromClient.Length);
            //
            //         Console.WriteLine("Message Received");
            //
            //         string s = Encoding.ASCII.GetString(dataFromClient, 0, bytesRead);
            //
            //         Packeged inc = JsonSerializer.Deserialize<Packeged>(s);
            //         
            //         if (inc.type.Equals("request"))
            //         {
            //             IList<Message> messages = await model.getMessages();
            //             Packeged packeged = new Packeged {type = "list", messages = messages};
            //             string packageserial = JsonSerializer.Serialize(packeged);
            //             byte[] dataToClient = Encoding.ASCII.GetBytes(packageserial);
            //             stream.Write(dataToClient, 0, dataToClient.Length);
            //         }
            //         else if(inc.type.Equals("message"))
            //         {
            //             Packeged packeged = JsonSerializer.Deserialize<Packeged>(s);
            //             await model.SendReceived(packeged.Message);
            //             ClientConnector.BroadCastReceivedMessage(packeged.Message);
            //         }
            //     }
            //     catch (Exception)
            //     {
            //         Client.Close();
            //     }
            // }
        }

        public async void ReceiveBroadcastMessage(Message message)
        {
            try
            {
                NetworkStream stream = Client.GetStream();
                Packeged packeged = new Packeged {type = "message", Message = message};
                
                string packageserial = JsonSerializer.Serialize(packeged);
                
                byte[] dataToClient = Encoding.ASCII.GetBytes(packageserial);

                stream.Write(dataToClient, 0, dataToClient.Length);
            }
            catch (Exception)
            {
                Client.Close();
            }
        }
    }
}