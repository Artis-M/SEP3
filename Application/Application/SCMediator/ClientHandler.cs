using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Tier2.Model;

namespace Application.SCMediator {
    public class ClientHandler {
        public TcpClient Client;
        public NetworkStream netStream;
        public ChatServiceImp ServiceImp;
        public Tier2.Model.Model model;


        public ClientHandler(NetworkStream stream, TcpClient client, ChatServiceImp service) {
            Console.WriteLine("Client Handler Starting!");
            Client = client;
            netStream = stream;
            ServiceImp = service;
            Thread t1 = new Thread(MessageHandler);
            t1.Start();
        }

        public void MessageHandler() {
            while (true) {
                netStream = Client.GetStream();
                byte[] dataFromServer = new byte[1024];
                int bytesRead = netStream.Read(dataFromServer, 0, dataFromServer.Length);
                string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
                Console.WriteLine(response);
                UPSBox upsdelivery = JsonSerializer.Deserialize<UPSBox>(response);
                Console.WriteLine("UPSBox " + upsdelivery.type + "  =>>>  " + upsdelivery.type);
                if (upsdelivery.type.Equals("Chatroom")) {
                    // model.getChatroom(upsdelivery.JSonThing);
                }
                else if (upsdelivery.type.Equals("ChatroomList")) {
                    
                }
                else if (upsdelivery.type.Equals("UserCredentials")) {
                    // model.ProcessCredentials(upsdelivery.JSonThing);
                }
                else if (upsdelivery.type.Equals("UList")) {
                    // pass list to model manager
                }
                else {
                    //error 
                }
            }
        }
    }
}

// 1 - chatroom | 2 - user credentials | 3 - user list
