using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Model;
using Microsoft.AspNetCore.Mvc;

namespace Application.SCMediator
{
    public class ServiceImpl
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        

        public void startConnection()
        {
            tcpClient = new TcpClient("127.0.0.1", 2910); 
            stream = tcpClient.GetStream();
        }

        public ServiceImpl()
        {
            startConnection();
          
        }

        public async Task requestUser(string username)
        {
            string message = "Hello from client";
            byte[] dataToServer = Encoding.ASCII.GetBytes(message);
            stream.Write(dataToServer, 0, dataToServer.Length);
            Console.Out.WriteLine("send");
            
            byte[] fromServer = new byte[1024];
            int bytesRead = stream.Read(fromServer, 0, fromServer.Length);
            string response = Encoding.ASCII.GetString(fromServer, 0, bytesRead);
            Console.WriteLine(response);

        }
    }
}