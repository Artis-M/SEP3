using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Bson;
using Tier2.Model;

namespace Application.SCMediator {
    public class ChatServiceImp {
        private TcpClient Client;
        private NetworkStream stream;
        private ClientHandler clientHandler;
        public Action<Message> newMessage;
        public Action<IList<Message>> messageHistory;
        private int PORT = 8888888;

        public ChatServiceImp() {
             connectToServer("localhost", PORT);
        }

        public async Task connectToServer(string ip, int port) {
            try {
                Client = new TcpClient(ip, port);
                clientHandler = new ClientHandler(stream, Client, this);
            }
            catch (Exception e) {
                Console.WriteLine("Connecting to server. Retrying.");
                connectToServer("localhost", PORT);
            }

            stream = Client.GetStream();
        }

        public async Task Send(CommandLine command) {
            string upsdelivery = JsonSerializer.Serialize(command);
            stream = Client.GetStream();
            byte[] dataToServer = Encoding.ASCII.GetBytes(upsdelivery);
            stream.Write(dataToServer, 0, dataToServer.Length);
        }
        // ------------------- //
        //        send         //
        // ------------------- //
        public async Task sendMessage(Message message, ObjectId chatroomID) {
            //to be changed
            CommandLine command = new CommandLine { Command = "Message", variableUser = message.authorID, variableChatroom = chatroomID, SpecificOrder = message.message};
            await Send(command);
        }

        public async Task sendNewChatroom(ObjectId userID, ObjectId chatroomID, String name) {
            CommandLine command = new CommandLine { Command = "ChatroomNew", variableUser = userID, variableChatroom = chatroomID, SpecificOrder = name };
            await Send(command);
        }

        public async Task sendChatroomUpdate() {
            CommandLine command = new CommandLine { Command = "ChatroomUpdate" };
            await Send(command);
        }

        public async Task sendNewUser() {
            CommandLine command = new CommandLine { Command = "UserNew" };
            await Send(command);
        }
        public async Task sendUserUpdate() {
            CommandLine command = new CommandLine { Command = "UserUpdate" };
            await Send(command);
        }
        // ------------------- //
        //      requests       //
        // ------------------- //
        public async Task requestUser(ObjectId userID) {
            CommandLine command = new CommandLine { Command = "REQUEST-User", variableUser = userID };
            await Send(command);
        }
        public async Task requestChatroom(ObjectId chatroomID) {
            CommandLine command = new CommandLine { Command = "REQUEST-Chatroom", variableChatroom = chatroomID };
            await Send(command);
        }
        public async Task requestUserCredentials(ObjectId userID) {
            CommandLine command = new CommandLine { Command = "REQUEST-UserCredentials" };
            await Send(command);
        }
    }
}
