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
                //clientHandler = new ClientHandler(stream, Client, modelManager);
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
           // byte[] dataToServer = Encoding.ASCII.GetBytes(upsdelivery);
           //stream.Write(dataToServer, 0, dataToServer.Length);
           int toSendLen = System.Text.Encoding.ASCII.GetByteCount(upsdelivery);
           byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(upsdelivery);
           byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
           stream.Write(toSendLenBytes);
           stream.Write(toSendBytes,0,toSendLen);

        }

        // ------------------- //
        //        send         //
        // ------------------- //
        public async Task sendMessage(Message message, string chatroomID)
        {
            string messageSerialized = JsonSerializer.Serialize(message);
            CommandLine command = new CommandLine
            {
                Command = "NewMessage", variableUser = message.authorID, variableChatroom = chatroomID,
                SpecificOrder = messageSerialized
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

        public async Task addTopicToUser(string userId, string topic)
        {
            CommandLine sendCommand = new CommandLine();
            sendCommand.Command = "AddTopicToUser";
            sendCommand.variableUser = userId;
            sendCommand.SpecificOrder = topic;
            Console.Out.WriteLine("AddTopuc");
            await Send(sendCommand);
        }
        public async Task removeTopicfromUser(string userId, string topic)
        {
            CommandLine sendCommand = new CommandLine();
            sendCommand.Command = "removeTopicFromUser";
            sendCommand.variableUser = userId;
            sendCommand.SpecificOrder = topic;
            await Send(sendCommand);
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

        public async Task JoinChatroom(string chatroomID, string userID)
        {
            Console.Out.WriteLine("JOIn CHAT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            CommandLine command = new CommandLine
                {Command = "JOIN-Chatroom", variableChatroom = chatroomID, variableUser = userID};
            await Send(command);
        }

        // ------------------- //
        //      deletes       //
        // ------------------- //
        public async Task DeleteChatroom(string chatroomID)
        {
            Console.Out.WriteLine(chatroomID);
            CommandLine command = new CommandLine {Command = "DELETE-Chatroom", SpecificOrder = chatroomID};
            
            await Send(command);
        }

        public async Task DeletePrivateChatroom(string userID, string friendID)
        {
            CommandLine command = new CommandLine{Command = "DELETE-PrivateChatroom",SpecificOrder = userID,variableUser = friendID};
            await Send(command);
        }
        
        public async Task DeleteUser(string userID)
        {
            CommandLine command = new CommandLine {Command = "DELETE-User", variableUser = userID};
            Console.Out.WriteLine("DELETE USER "+userID);
            await Send(command);
        }

        public async Task DeleteTopic(string topicID)
        {
            CommandLine command = new CommandLine {Command = "DELETE-Topic", SpecificOrder = topicID};
            await Send(command);
        }

        public async Task LeaveChatroom(string chatroomID, string userID)
        {
            CommandLine command = new CommandLine
                {Command = "LEAVE-Chatroom", variableChatroom = chatroomID, variableUser = userID};
            await Send(command);
        }

        // ------------------- //
        //      requests       //
        // ------------------- //
        public async Task<List<Account>> requestUsers()
        {
            CommandLine command = new CommandLine {Command = "REQUEST-UserCredentials"};
            await Send(command);
            
            
            byte[] rcvLenBytes = new byte[4];
            stream.Read(rcvLenBytes);
            int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
            byte[] dataFromServer = new byte[rcvLen];
            int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
            string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
            CommandLine upsdelivery = JsonSerializer.Deserialize<CommandLine>(response);
            if (upsdelivery.Command == "UserCredentials")
            {
                List<Account> accounts = JsonSerializer.Deserialize<List<Account>>(upsdelivery.SpecificOrder);
                return accounts;
            }
            else
            {
                //wait again for data from server, check again
                return null;
            }
        }

        public async Task<List<Chatroom>> requestUsersChatroom(string id)
        {
            CommandLine command = new CommandLine {Command = "REQUEST-ChatroomByUser", variableUser = id};

            await Send(command);
            /* move out */
            byte[] rcvLenBytes = new byte[4];
            stream.Read(rcvLenBytes);
            int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
            byte[] dataFromServer = new byte[rcvLen];
            int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
            string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
            Console.WriteLine(response);
            CommandLine upsdelivery = JsonSerializer.Deserialize<CommandLine>(response);
            /* to here */
            if (upsdelivery.Command.Equals("ChatroomByUser"))
            {
                List<Chatroom> chatrooms = JsonSerializer.Deserialize<List<Chatroom>>(upsdelivery.SpecificOrder);
                return chatrooms;
            }
            else
            {
                return null;
            }
        }


        public async Task<Account> requestUser(string username)
        {
            CommandLine command = new CommandLine {Command = "REQUEST-User", variableUser = username};
            await Send(command);

            byte[] rcvLenBytes = new byte[4];
            stream.Read(rcvLenBytes);
            int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
            byte[] dataFromServer = new byte[rcvLen];
            int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
            string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
            CommandLine upsdelivery = JsonSerializer.Deserialize<CommandLine>(response);
            if (upsdelivery.Command.Equals("OneUserCredential"))
            {
                Account account = JsonSerializer.Deserialize<Account>(upsdelivery.SpecificOrder);
                return account;
            }
            else
            {
                return null;
            }
        }

        public async Task<Chatroom> getPrivateChatroom(string user, string user1)
        {
            CommandLine command = new CommandLine {Command = "REQUEST-PrivateCHatroom", variableUser = user,SpecificOrder = user1};
            await Send(command);
            Chatroom room = new Chatroom();
            byte[] rcvLenBytes = new byte[4];
            stream.Read(rcvLenBytes);
            int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
            byte[] dataFromServer = new byte[rcvLen];
            int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
            string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
            CommandLine upsdelivery = JsonSerializer.Deserialize<CommandLine>(response);
            if (upsdelivery.Command.Equals("REQUEST-PrivateCHatroom"))
            {
                room = JsonSerializer.Deserialize<Chatroom>(upsdelivery.SpecificOrder);
                return room;
            }

            return null;
        }
        public async Task<Account> requestUserByID(string userID)
        {
            CommandLine command = new CommandLine {Command = "REQUEST-UserByID", variableUser = userID};
            await Send(command);

            byte[] rcvLenBytes = new byte[4];
            stream.Read(rcvLenBytes);
            int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
            byte[] dataFromServer = new byte[rcvLen];
            int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
            string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
            CommandLine upsdelivery = JsonSerializer.Deserialize<CommandLine>(response);
            if (upsdelivery.Command.Equals("OneUserCredentialByID"))
            {
                Account account = JsonSerializer.Deserialize<Account>(upsdelivery.SpecificOrder);
                return account;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Chatroom>> requestChatrooms()
        {
            CommandLine command = new CommandLine {Command = "REQUEST-Chatroom-ALL"};
            await Send(command);

            byte[] rcvLenBytes = new byte[4];
            stream.Read(rcvLenBytes);
            int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
            Console.WriteLine(rcvLen);
            byte[] dataFromServer = new byte[rcvLen];
            int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
            string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
            CommandLine upsdelivery = JsonSerializer.Deserialize<CommandLine>(response);
            if (upsdelivery.Command.Equals("AllChatrooms"))
            {
                List<Chatroom> chatrooms = JsonSerializer.Deserialize<List<Chatroom>>(upsdelivery.SpecificOrder);
                return chatrooms;
            }
            return null;
        }

        public async Task AddFriend(List<User> users)
        {
            string serialized = JsonSerializer.Serialize(users);
            CommandLine commandLine = new CommandLine
            {
                Command = "AddFriends",
                SpecificOrder = serialized
            };
            await Send(commandLine);
        }
        
        public async Task removeFriend(string userId,string friendId)
        {
            
            CommandLine commandLine = new CommandLine
            {
                Command = "removeFriend",
                variableUser = userId,
                SpecificOrder = friendId
            };
            Console.Out.WriteLine($"remove {friendId} from {userId}");
            await Send(commandLine);
        }
        


        public async Task<List<Chatroom>> requestChatroomsByTopic(string topic)
        {
            CommandLine commandLine = new CommandLine
            {
                Command = "ChatroomsByTopic",
                SpecificOrder = topic
            };
            await Send(commandLine);
            byte[] rcvLenBytes = new byte[4];
            stream.Read(rcvLenBytes);
            int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
            byte[] dataFromServer = new byte[rcvLen];
            int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
            string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
            CommandLine upsdelivery = JsonSerializer.Deserialize<CommandLine>(response);
            if (upsdelivery.Command.Equals("ChatroomsByTopic"))
            {
                Console.Out.WriteLine(upsdelivery.SpecificOrder);
                List<Chatroom> chatrooms = JsonSerializer.Deserialize<List<Chatroom>>(upsdelivery.SpecificOrder);
                return chatrooms;
            }

            return null;
        }
        /*public async Task<List<Topic>> requestTopics()
        {
            CommandLine command = new CommandLine {Command = "REQUEST-Topic-ALL"};
            await Send(command);

            byte[] dataFromServer = new byte[4048];
            int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
            string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
            Console.WriteLine(response);
            CommandLine upsdelivery = JsonSerializer.Deserialize<CommandLine>(response);

            List<Topic> topics = JsonSerializer.Deserialize<List<Topic>>(response);
            return topics;
        }*/
    }
}