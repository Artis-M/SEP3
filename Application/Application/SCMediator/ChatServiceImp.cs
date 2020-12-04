using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Model;
using MongoDB.Bson;
using Tier2.Model;

namespace Application.SCMediator {
    public class ChatServiceImp {
        private TcpClient Client;
        private NetworkStream stream;
        private ClientHandler clientHandler;
        private int PORT = 8443;

        public ChatServiceImp() {
             connectToServer("localhost", PORT);
             
        }

        public async Task connectToServer(string ip, int port) {
            try {
                Client = new TcpClient(ip, port);
                //clientHandler = new ClientHandler(stream, Client, this);
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
        public async Task sendMessage(Message message, string chatroomID) {    
            CommandLine command = new CommandLine { Command = "Message", variableUser = message.authorID, variableChatroom = chatroomID, SpecificOrder = message.message};
            await Send(command);
        }

        public async Task sendNewChatroom(Chatroom chatroom) {
            string serialChatroom = JsonSerializer.Serialize(chatroom);
            CommandLine command = new CommandLine { Command = "ChatroomNew", SpecificOrder = serialChatroom };
            await Send(command);
        }

        public async Task sendChatroomUpdate(Chatroom chatroom) {
            string serialChatroom = JsonSerializer.Serialize(chatroom);
            CommandLine command = new CommandLine { Command = "ChatroomUpdate", SpecificOrder = serialChatroom };
            await Send(command);
        }

        public async Task sendNewUser(Account account) {
            string serialUser = JsonSerializer.Serialize(account);
            CommandLine command = new CommandLine { Command = "UserNew", SpecificOrder = serialUser };
            await Send(command);
        }
        public async Task sendUserUpdate(Account account) {
            string serialUserUpdate = JsonSerializer.Serialize(account);
            CommandLine command = new CommandLine { Command = "UserUpdate", SpecificOrder = serialUserUpdate };
            await Send(command);
        }
        public async Task sendNewTopic(Topic topic) {
            string serialTopic = JsonSerializer.Serialize(topic);
            CommandLine command = new CommandLine { Command = "TopicNew", SpecificOrder = serialTopic };
            await Send(command);
        }
        public async Task sendTopicUpdate(Topic topic) {
            string serialTopic = JsonSerializer.Serialize(topic);
            CommandLine command = new CommandLine { Command = "TopicUpdate", SpecificOrder = serialTopic };
            await Send(command);
        }
        // ------------------- //
        //      deletes       //
        // ------------------- //
        public async Task DeleteChatroom(string chatroomID)
        {
            CommandLine command = new CommandLine { Command = "DELETE-Chatroom", SpecificOrder = chatroomID };
            await Send(command);
        }

        // ------------------- //
        //      requests       //
        // ------------------- //
        public async Task requestUserCredentials() {
            CommandLine command = new CommandLine { Command = "REQUEST-UserCredentials" };
            await Send(command);
        }
        public async Task requestChatrooms() {
            CommandLine command = new CommandLine { Command = "REQUEST-Chatroom-ALL" };
            await Send(command);
        }
        public async Task requestTopics() {
            CommandLine command = new CommandLine { Command = "REQUEST-Topic-ALL" };
            await Send(command);
        }
    }
}
