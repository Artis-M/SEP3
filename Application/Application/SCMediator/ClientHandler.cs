using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;

namespace Application.SCMediator {
    public class ClientHandler {
        public TcpClient Client;
        public NetworkStream netStream;
        public Application.Models.Model model;


        public ClientHandler(NetworkStream stream, TcpClient client, ModelManager modelManager) {
            Console.WriteLine("Client Handler Starting!");
            
            Client = client;
            
            netStream = stream;
            Thread t1 = new Thread(MessageHandler);
            t1.Start();
            this.model = modelManager;
        }

        public void MessageHandler() {
            while (true) {
                netStream = Client.GetStream();
                byte[] dataFromServer = new byte[4048];
                int bytesRead = netStream.Read(dataFromServer, 0, dataFromServer.Length);
                string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
               // Console.WriteLine(response);
                CommandLine upsdelivery = JsonSerializer.Deserialize<CommandLine>(response);
               // Console.WriteLine("UPSBox " + upsdelivery.Command + "  =>>>  " + upsdelivery.Command);
                if (upsdelivery.Command.Equals("Chatroom")) {
                    // model.getChatroom(upsdelivery.JSonThing);
                }
                else if (upsdelivery.Command.Equals("ChatroomList")) { model.ProcessChatrooms(upsdelivery.SpecificOrder);
                }
                else if (upsdelivery.Command.Equals("UserCredentials")) {
                  //  Console.Out.WriteLine(upsdelivery.SpecificOrder);
                    model.ProcessCredentials(upsdelivery.SpecificOrder);
                }
                else if (upsdelivery.Command.Equals("TopicList")) {
                    model.ProcessTopics(upsdelivery.SpecificOrder);
                }
                else {
                    //error 
                }
            }
        }
    }
}

// 1 - chatroom | 2 - user credentials | 3 - user list
