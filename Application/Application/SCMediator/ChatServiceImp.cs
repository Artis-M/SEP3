using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Models;

namespace Application.SCMediator
{
    /// <summary>
    /// This class realizes the connection to Tier 3 through sockets and sends-recieves infomration
    /// </summary>
    public class ChatServiceImp
    {
        private TcpClient Client;
        private NetworkStream stream;
        private int PORT = 2000;

        public ChatServiceImp(ModelManager modelManager)
        {
            ConnectToServer("localhost", PORT, modelManager);
        }

        /// <summary>
        /// Connecting to server
        /// </summary>
        /// <param name="ip">Ip address to connect to</param>
        /// <param name="port">Port number to use during connection</param>
        /// <param name="modelManager">instance of model to call corresponding methods</param>
        /// <returns>no return</returns>
        public async Task ConnectToServer(string ip, int port, ModelManager modelManager)
        {
            try
            {
                Client = new TcpClient(ip, port);
            }
            catch (Exception e)
            {
                Console.WriteLine("Connecting to server. Retrying.");
                await ConnectToServer("localhost", PORT, modelManager);
            }

            stream = Client.GetStream();
        }

        /// <summary>
        /// General send method to send data to Tier 3
        /// </summary>
        /// <param name="command">Object that contains the command itself as well as additional values</param>
        /// <returns>no return</returns>
        public async Task Send(CommandLine command)
        {
            string upsdelivery = JsonSerializer.Serialize(command);
            stream = Client.GetStream();
            int toSendLen = System.Text.Encoding.ASCII.GetByteCount(upsdelivery);
            byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(upsdelivery);
            byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
            stream.Write(toSendLenBytes);
            stream.Write(toSendBytes, 0, toSendLen);
        }

        // ------------------- //
        //        send         //
        // ------------------- //
        /// <summary>
        /// Sending a message to a chatroom
        /// </summary>
        /// <param name="message">Message being sent</param>
        /// <param name="chatroomID">Chatroom to send the message to</param>
        /// <returns>no return</returns>
        public async Task SendMessage(Message message, string chatroomID)
        {
            string messageSerialized = JsonSerializer.Serialize(message);
            CommandLine command = new CommandLine
            {
                Command = "NewMessage", variableUser = message.authorID, variableChatroom = chatroomID,
                SpecificOrder = messageSerialized
            };
            await Send(command);
        }

        /// <summary>
        /// Adding a new chatroom
        /// </summary>
        /// <param name="chatroom">Chatroom to be added</param>
        /// <returns>no return</returns>
        public async Task SendNewChatroom(Chatroom chatroom)
        {
            string serialChatroom = JsonSerializer.Serialize(chatroom);
            CommandLine command = new CommandLine {Command = "ChatroomNew", SpecificOrder = serialChatroom};
            await Send(command);
        }

        /// <summary>
        /// Updating a chatroom
        /// </summary>
        /// <param name="chatroom">Chatroom to be updated with its new values</param>
        /// <returns>no return</returns>
        public async Task SendChatroomUpdate(Chatroom chatroom)
        {
            string serialChatroom = JsonSerializer.Serialize(chatroom);
            CommandLine command = new CommandLine {Command = "ChatroomUpdate", SpecificOrder = serialChatroom};
            await Send(command);
        }

        /// <summary>
        /// Registering a new user
        /// </summary>
        /// <param name="account">Account to be registered</param>
        /// <returns>no return</returns>
        public async Task SendNewUser(Account account)
        {
            string serialUser = JsonSerializer.Serialize(account);
            CommandLine command = new CommandLine {Command = "UserNew", SpecificOrder = serialUser};
            await Send(command);
        }

        /// <summary>
        /// Updating an existing user with new information
        /// </summary>
        /// <param name="account">Account to be updated</param>
        /// <returns>no return</returns>
        public async Task SendUserUpdate(Account account)
        {
            string serialUserUpdate = JsonSerializer.Serialize(account);
            CommandLine command = new CommandLine {Command = "UserUpdate", SpecificOrder = serialUserUpdate};
            await Send(command);
        }

        /// <summary>
        /// Adding a topic to a user
        /// </summary>
        /// <param name="userId">To whom the topic is added</param>
        /// <param name="topic">The topic being added</param>
        /// <returns>no return</returns>
        public async Task AddTopicToUser(string userId, string topic)
        {
            CommandLine sendCommand = new CommandLine();
            sendCommand.Command = "AddTopicToUser";
            sendCommand.variableUser = userId;
            sendCommand.SpecificOrder = topic;
            Console.Out.WriteLine("AddTopuc");
            await Send(sendCommand);
        }

        /// <summary>
        /// Removing a topic from a user
        /// </summary>
        /// <param name="userId">From whom it is being removed</param>
        /// <param name="topic">What is being removed</param>
        /// <returns>no return</returns>
        public async Task RemoveTopicfromUser(string userId, string topic)
        {
            CommandLine sendCommand = new CommandLine();
            sendCommand.Command = "removeTopicFromUser";
            sendCommand.variableUser = userId;
            sendCommand.SpecificOrder = topic;
            await Send(sendCommand);
        }

        /// <summary>
        /// Joining a chatroom
        /// </summary>
        /// <param name="chatroomID">Chat room to be joine</param>
        /// <param name="userID">Who is joining</param>
        /// <returns>no return</returns>
        public async Task JoinChatroom(string chatroomID, string userID)
        {
            CommandLine command = new CommandLine
                {Command = "JOIN-Chatroom", variableChatroom = chatroomID, variableUser = userID};
            await Send(command);
        }

        // ------------------- //
        //      deletes       //
        // ------------------- //
        /// <summary>
        /// Deleting a chat room
        /// </summary>
        /// <param name="chatroomID">Chat room to be deleted</param>
        /// <returns>no return</returns>
        public async Task DeleteChatroom(string chatroomID)
        {
            CommandLine command = new CommandLine {Command = "DELETE-Chatroom", SpecificOrder = chatroomID};
            await Send(command);
        }

        /// <summary>
        /// Deleting a private chatroom between two friends
        /// </summary>
        /// <param name="userID">Friend 1</param>
        /// <param name="friendID">Friend 2</param>
        /// <returns>no return</returns>
        public async Task DeletePrivateChatroom(string userID, string friendID)
        {
            CommandLine command = new CommandLine
                {Command = "DELETE-PrivateChatroom", SpecificOrder = userID, variableUser = friendID};
            await Send(command);
        }

        /// <summary>
        /// Deleting a user
        /// </summary>
        /// <param name="userID">User to be deleted</param>
        /// <returns>no return</returns>
        public async Task DeleteUser(string userID)
        {
            CommandLine command = new CommandLine {Command = "DELETE-User", variableUser = userID};
            Console.Out.WriteLine("DELETE USER " + userID);
            await Send(command);
        }

        /// <summary>
        /// Deleting a topic
        /// </summary>
        /// <param name="topicID">Topic to be deleted</param>
        /// <returns>no return</returns>
        public async Task DeleteTopic(string topicID)
        {
            CommandLine command = new CommandLine {Command = "DELETE-Topic", SpecificOrder = topicID};
            await Send(command);
        }

        /// <summary>
        /// Leaving a chatroom
        /// </summary>
        /// <param name="chatroomID">Chat room to be left</param>
        /// <param name="userID">User wanting to leave</param>
        /// <returns>no return</returns>
        public async Task LeaveChatroom(string chatroomID, string userID)
        {
            CommandLine command = new CommandLine
                {Command = "LEAVE-Chatroom", variableChatroom = chatroomID, variableUser = userID};
            await Send(command);
        }

        // ------------------- //
        //      requests       //
        // ------------------- //
        /// <summary>
        /// Requesting all Users' information
        /// </summary>
        /// <returns>A List of all Accounts present</returns>
        public async Task<List<Account>> RequestUsers()
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
                return null;
            }
        }

        /// <summary>
        /// Requesting a chat room by a User's ID
        /// </summary>
        /// <param name="id">ID of User who's chat rooms to be retrieved</param>
        /// <returns>List of chat rooms the User is part of</returns>
        public async Task<List<Chatroom>> RequestUsersChatroom(string id)
        {
            CommandLine command = new CommandLine {Command = "REQUEST-ChatroomByUser", variableUser = id};

            await Send(command);

            byte[] rcvLenBytes = new byte[4];
            stream.Read(rcvLenBytes);
            int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
            byte[] dataFromServer = new byte[rcvLen];
            int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
            string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
            Console.WriteLine(response);
            CommandLine upsdelivery = JsonSerializer.Deserialize<CommandLine>(response);

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

        /// <summary>
        /// Requesting a user by its username
        /// </summary>
        /// <param name="username">Username of User</param>
        /// <returns>Account that has the given username</returns>
        public async Task<Account> RequestUser(string username)
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

        /// <summary>
        /// Requesting a private chat room of two friends
        /// </summary>
        /// <param name="user">Participant 1</param>
        /// <param name="user1">Participant 2</param>
        /// <returns>Chat room of two users</returns>
        public async Task<Chatroom> GetPrivateChatroom(string user, string user1)
        {
            CommandLine command = new CommandLine
                {Command = "REQUEST-PrivateCHatroom", variableUser = user, SpecificOrder = user1};
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

        /// <summary>
        /// Requesting a user by its ID
        /// </summary>
        /// <param name="userID">User's ID that is requested</param>
        /// <returns>Account that has the given ID</returns>
        public async Task<Account> RequestUserById(string userID)
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

        /// <summary>
        /// Requesting all the chat rooms
        /// </summary>
        /// <returns>All the chat rooms present</returns>
        public async Task<List<Chatroom>> RequestChatrooms()
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

        /// <summary>
        /// Adding a friend
        /// </summary>
        /// <param name="users">List containing to users who should become each others friends</param>
        /// <returns><no return/returns>
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

        /// <summary>
        /// Removing a friend
        /// </summary>
        /// <param name="userId">Friend 1 to be removed from the other friend</param>
        /// <param name="friendId">Friend 2 to be removed from the other friend</param>
        /// <returns>no return</returns>
        public async Task RemoveFriend(string userId, string friendId)
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

        /// <summary>
        /// Requesting all the chat rooms that have the given topic
        /// </summary>
        /// <param name="topic">Topic that should be contained by the chat rooms</param>
        /// <returns>List of chat rooms that have the given topic</returns>
        public async Task<List<Chatroom>> RequestChatroomsByTopic(string topic)
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
    }
}