using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Models;
using MongoDB.Bson;

namespace Application.SCMediator
{
    public class ChatServiceImp
    {
        private TcpClient Client;
        private NetworkStream stream;
        private ClientHandler clientHandler;
        private int PORT = 2000;

        public ChatServiceImp(ModelManager modelManager)
        {
            connectToServer("localhost", PORT, modelManager);
        }


        public async Task connectToServer(string ip, int port, ModelManager modelManager)
        {
            try
            {
                Client = new TcpClient(ip, port);
                clientHandler = new ClientHandler(stream, Client, modelManager);
            }
            catch (Exception e)
            {
                Console.WriteLine("Connecting to server. Retrying.");
                connectToServer("localhost", PORT, modelManager);
            }

            stream = Client.GetStream();
        }

        public async Task Send(CommandLine command)
        {
            string upsdelivery = JsonSerializer.Serialize(command);
            stream = Client.GetStream();
            byte[] dataToServer = Encoding.ASCII.GetBytes(upsdelivery);
            stream.Write(dataToServer, 0, dataToServer.Length);
        }

        // ------------------- //
        //        send         //
        // ------------------- //
        public async Task sendMessage(Message message, string chatroomID)
        {
            CommandLine command = new CommandLine
            {
                Command = "Message", variableUser = message.authorID, variableChatroom = chatroomID,
                SpecificOrder = message.message
            };
            await Send(command);
        }

        public async Task sendNewChatroom(Chatroom chatroom)
        {
            string serialChatroom = JsonSerializer.Serialize(chatroom);
            CommandLine command = new CommandLine {Command = "ChatroomNew", SpecificOrder = serialChatroom};
            await Send(command);
        }

        public async Task sendChatroomUpdate(Chatroom chatroom)
        {
            string serialChatroom = JsonSerializer.Serialize(chatroom);
            CommandLine command = new CommandLine {Command = "ChatroomUpdate", SpecificOrder = serialChatroom};
            await Send(command);
        }

        public async Task sendNewUser(Account account)
        {
            string serialUser = JsonSerializer.Serialize(account);
            CommandLine command = new CommandLine {Command = "UserNew", SpecificOrder = serialUser};
            await Send(command);
        }

        public async Task sendUserUpdate(Account account)
        {
            string serialUserUpdate = JsonSerializer.Serialize(account);
            CommandLine command = new CommandLine {Command = "UserUpdate", SpecificOrder = serialUserUpdate};
            await Send(command);
        }

        public async Task sendNewTopic(Topic topic)
        {
            string serialTopic = JsonSerializer.Serialize(topic);
            CommandLine command = new CommandLine {Command = "TopicNew", SpecificOrder = serialTopic};
            await Send(command);
        }

        public async Task sendTopicUpdate(Topic topic)
        {
            string serialTopic = JsonSerializer.Serialize(topic);
            CommandLine command = new CommandLine {Command = "TopicUpdate", SpecificOrder = serialTopic};
            await Send(command);
        }

        // ------------------- //
        //      deletes       //
        // ------------------- //
        public async Task DeleteChatroom(string chatroomID)
        {
            CommandLine command = new CommandLine {Command = "DELETE-Chatroom", SpecificOrder = chatroomID};
            await Send(command);
        }

        public async Task DeleteUser(string userID)
        {
            CommandLine command = new CommandLine {Command = "DELETE-User", SpecificOrder = userID};
            await Send(command);
        }

        // ------------------- //
        //      requests       //
        // ------------------- //
        public async Task requestUserCredentials()
        {
            CommandLine command = new CommandLine {Command = "REQUEST-UserCredentials"};
            await Send(command);
        }

        public async Task<Chatroom> requestChatroom(string id)
        {
            CommandLine command = new CommandLine {Command = "REQUEST-Chatroom", variableChatroom = id};
            await Send(command);
            byte[] dataFromServer = new byte[4048];
            int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
            string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
            Console.WriteLine(response);
            CommandLine upsdelivery = JsonSerializer.Deserialize<CommandLine>(response);
            Chatroom chatroom = JsonSerializer.Deserialize<Chatroom>(upsdelivery.SpecificOrder);
            return chatroom;
        }


        public async Task<Account> requestUser(string username)
        {
            CommandLine command = new CommandLine {Command = "REQUEST-User", variableUser = username};
            await Send(command);

            byte[] dataFromServer = new byte[4048];
            int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
            string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
            Console.WriteLine(response);
            CommandLine upsdelivery = JsonSerializer.Deserialize<CommandLine>(response);
            Account account = JsonSerializer.Deserialize<Account>(upsdelivery.SpecificOrder);
            return account;
        }

        public async Task<List<Chatroom>> requestChatrooms()
        {
            CommandLine command = new CommandLine {Command = "REQUEST-Chatroom-ALL"};
            await Send(command);
            byte[] dataFromServer = new byte[4048];
            int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
            string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
            Console.WriteLine(response);
            CommandLine upsdelivery = JsonSerializer.Deserialize<CommandLine>(response);
            List<Chatroom> chatrooms = JsonSerializer.Deserialize<List<Chatroom>>(upsdelivery.SpecificOrder);
            return chatrooms;
        }

        public async Task requestTopics()
        {
            CommandLine command = new CommandLine {Command = "REQUEST-Topic-ALL"};
            await Send(command);
        }
    }
}