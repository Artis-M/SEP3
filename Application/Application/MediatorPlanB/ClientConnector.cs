﻿using System;
using System.Net;
using System.Net.Sockets;
using Tier2.Model;

namespace Tier2.MediatorPlanB
{
    public class ClientConnector
    {
        private IPAddress ip;
        private TcpListener listener;
        public Action<Message> BroadcastMessage;
        public void StartServer()
        {
            Console.WriteLine("Starting Server");
            ip = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(ip, 1337);
            listener.Start();
            Console.WriteLine("Server started");
            
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected.");
                ClientHandler clientHandler = new ClientHandler(client, this);
                this.BroadcastMessage += clientHandler.ReceiveBroadcastMessage;
            }
        }

        public void BroadCastReceivedMessage(Message message)
        {
            BroadcastMessage?.Invoke(message);
        }

    }
}