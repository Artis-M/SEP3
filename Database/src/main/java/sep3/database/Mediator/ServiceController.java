package sep3.database.Mediator;


import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import org.bson.types.ObjectId;
import sep3.database.Model.Account;
import sep3.database.Model.Chatroom;
import sep3.database.Model.Message;
import sep3.database.Model.User;
import sep3.database.Persistance.*;

import javax.net.ssl.SSLServerSocketFactory;
import java.io.*;
import java.lang.reflect.Type;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;

public class ServiceController implements Runnable
{
    private int PORT = 2000;
    private boolean running;
    private ServerSocket welcomeSocket;
    private UserDAO userDAO;
    private TopicDAO topicDAO;
    private ChatroomDAO chatroomDAO;
    private Gson gson;

    public ServiceController() throws IOException
    {
        gson = new Gson();
        this.userDAO = new UserDAOImpl();
        this.chatroomDAO = new ChatroomDAOImpl();
        this.topicDAO = new TopicDAOImpl();
        this.running = true;
        running = true;
        //removed ssl
        //welcomeSocket = (SSLServerSocketFactory.getDefault()).createServerSocket(PORT);
        welcomeSocket = new ServerSocket(PORT);
    }


    @Override
    public void run()
    {


        try
        {
            System.out.println("ServerSocket is ready for connection...");
            Socket socket = welcomeSocket.accept();
            InputStream inputStream = socket.getInputStream();
            OutputStream outputStream = socket.getOutputStream();
            System.out.println("Client Connected");
            while (running)
            {
                byte[] length = new byte[4];
                inputStream.read(length, 0, 4);
                int len = (((length[3] & 0xff) << 24) | ((length[2] & 0xff) << 16) |
                        ((length[1] & 0xff) << 8) | (length[0] & 0xff));
                byte[] lenbytes = new byte[len];
                int read = inputStream.read(lenbytes, 0, lenbytes.length);
                String request = new String(lenbytes, 0, read);


                CommandLine requestCommand = gson.fromJson(request, CommandLine.class);
               // System.out.println(requestCommand.getSpecificOrder());
                CommandLine responseCommand = new CommandLine();
                if (requestCommand.getCommand().equals("REQUEST-UserCredentials"))
                {


                    CommandLine commandLine1 = new CommandLine();
                    String response = gson.toJson(userDAO.getAllAccount());
                    commandLine1.setSpecificOrder(response);
                    System.out.println(response);
                    commandLine1.setCommand("UserCredentials");
                    String sendBack = gson.toJson(commandLine1);
                    byte[] toSendBytes = sendBack.getBytes();
                    int toSendLen = toSendBytes.length;
                    byte[] toSendLenBytes = new byte[4];
                    toSendLenBytes[0] = (byte)(toSendLen & 0xff);
                    toSendLenBytes[1] = (byte)((toSendLen >> 8) & 0xff);
                    toSendLenBytes[2] = (byte)((toSendLen >> 16) & 0xff);
                    toSendLenBytes[3] = (byte)((toSendLen >> 24) & 0xff);
                    byte[] responseAsBytes = sendBack.getBytes();
                    outputStream.write(toSendLenBytes);
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);


                } else if (requestCommand.getCommand().equals("REQUEST-User"))
                {

                    String response = gson.toJson(userDAO.getAccount(requestCommand.getVariableUser()));
                    System.out.println(response);
                    responseCommand.setSpecificOrder(response);
                    responseCommand.setCommand("OneUserCredential");
                    String sendBack = gson.toJson(responseCommand);
                    byte[] toSendBytes = sendBack.getBytes();
                    int toSendLen = toSendBytes.length;
                    byte[] toSendLenBytes = new byte[4];
                    toSendLenBytes[0] = (byte)(toSendLen & 0xff);
                    toSendLenBytes[1] = (byte)((toSendLen >> 8) & 0xff);
                    toSendLenBytes[2] = (byte)((toSendLen >> 16) & 0xff);
                    toSendLenBytes[3] = (byte)((toSendLen >> 24) & 0xff);
                    byte[] responseAsBytes = sendBack.getBytes();
                    outputStream.write(toSendLenBytes);
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);


                } else if (requestCommand.getCommand().equals("REQUEST-UserByID"))
                {

                    String response = gson.toJson(userDAO.getUser(requestCommand.getVariableUser()));
                    responseCommand.setSpecificOrder(response);
                    responseCommand.setCommand("OneUserCredentialByID");
                    String sendBack = gson.toJson(responseCommand);
                    byte[] toSendBytes = sendBack.getBytes();
                    int toSendLen = toSendBytes.length;
                    byte[] toSendLenBytes = new byte[4];
                    toSendLenBytes[0] = (byte)(toSendLen & 0xff);
                    toSendLenBytes[1] = (byte)((toSendLen >> 8) & 0xff);
                    toSendLenBytes[2] = (byte)((toSendLen >> 16) & 0xff);
                    toSendLenBytes[3] = (byte)((toSendLen >> 24) & 0xff);
                    byte[] responseAsBytes = sendBack.getBytes();
                    outputStream.write(toSendLenBytes);
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);

                } else if (requestCommand.getCommand().equals("UserNew"))
                {

                    Account account = gson.fromJson(requestCommand.getSpecificOrder(), Account.class);
                    userDAO.addAccount(account);
                } else if (requestCommand.getCommand().equals("REQUEST-Chatroom-ALL"))
                {

                    String response = gson.toJson(chatroomDAO.getAllChatrooms());
                    responseCommand.setSpecificOrder(response);
                    responseCommand.setCommand("AllChatrooms");
                    String sendBack = gson.toJson(responseCommand);
                    byte[] toSendBytes = sendBack.getBytes();
                    int toSendLen = toSendBytes.length;
                    byte[] toSendLenBytes = new byte[4];
                    toSendLenBytes[0] = (byte)(toSendLen & 0xff);
                    toSendLenBytes[1] = (byte)((toSendLen >> 8) & 0xff);
                    toSendLenBytes[2] = (byte)((toSendLen >> 16) & 0xff);
                    toSendLenBytes[3] = (byte)((toSendLen >> 24) & 0xff);
                    byte[] responseAsBytes = sendBack.getBytes();
                    outputStream.write(toSendLenBytes);
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);

                } else if (requestCommand.getCommand().equals("REQUEST-ChatroomByUser"))
                {


                    String response = gson.toJson(chatroomDAO.getChatroomByUserId(requestCommand.getVariableUser()));
                    responseCommand.setSpecificOrder(response);
                    responseCommand.setCommand("ChatroomByUser");
                    String sendBack = gson.toJson(responseCommand);
                    byte[] toSendBytes = sendBack.getBytes();
                    int toSendLen = toSendBytes.length;
                    byte[] toSendLenBytes = new byte[4];
                    toSendLenBytes[0] = (byte)(toSendLen & 0xff);
                    toSendLenBytes[1] = (byte)((toSendLen >> 8) & 0xff);
                    toSendLenBytes[2] = (byte)((toSendLen >> 16) & 0xff);
                    toSendLenBytes[3] = (byte)((toSendLen >> 24) & 0xff);
                    byte[] responseAsBytes = sendBack.getBytes();
                    outputStream.write(toSendLenBytes);
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);

                } else if (requestCommand.getCommand().equals("JOIN-Chatroom"))
                {
                    System.out.println("JOIN CHATROOM");
                    String userId = requestCommand.getVariableUser();
                    String chatroomId = requestCommand.getVariableChatroom();
                    chatroomDAO.joinChatroom(userId, chatroomId);
                } else if (requestCommand.getCommand().equals("LEAVE-Chatroom"))

                {

                    String userId = requestCommand.getVariableUser();
                    String chatroomId = requestCommand.getVariableChatroom();
                    chatroomDAO.leaveChatroom(userId, chatroomId);
                } else if (requestCommand.getCommand().equals("NewMessage"))
                {

                    String chatroomID = requestCommand.getVariableChatroom();
                    Message message = gson.fromJson(requestCommand.getSpecificOrder(), Message.class);
                    chatroomDAO.addMessageToChatroom(chatroomID, message);
                }
                else if(requestCommand.getCommand().equals("AddFriends")){
                    Type type = new TypeToken<ArrayList<User>>() {}.getType();
                    ArrayList<User> specificOrder = gson.fromJson(requestCommand.getSpecificOrder(), type);
                    User user1 = specificOrder.get(0);
                    User user2 = specificOrder.get(1);
                    userDAO.addFriend(user1.get_id(), user2.get_id());
                    userDAO.addFriend(user2.get_id(),user1.get_id());
                    Chatroom privateChat = new Chatroom();
                    ObjectId _id = new ObjectId();
                    privateChat.set_id(_id.toString());
                    privateChat.setType("private");
                    privateChat.addUser(user1);
                    privateChat.addUser(user2);
                    privateChat.setName(user1.getUsername() + " " + user2.getUsername());
                    privateChat.setOwner(user1.get_id());
                    chatroomDAO.AddChatroom(privateChat);
                }
                else if(requestCommand.getCommand().equals("removeFriend"))
                {
                    userDAO.removeFriend(requestCommand.getVariableUser(),requestCommand.getSpecificOrder());

                }
                else if(requestCommand.getCommand().equals("ChatroomNew")){

                    Chatroom chatroom = gson.fromJson(requestCommand.getSpecificOrder(),Chatroom.class);
                   // System.out.println();
                    chatroomDAO.AddChatroom(chatroom);
                }
                else if(requestCommand.getCommand().equals("REQUEST-PrivateCHatroom"))
                {
                    System.out.println(requestCommand);
                    Chatroom chatroom = new Chatroom();
                    chatroom = chatroomDAO.getPrivateChatroom(
                            requestCommand.getVariableUser(),requestCommand.getSpecificOrder());
                            String responseBack = gson.toJson(chatroom);
                    responseCommand.setSpecificOrder(responseBack);

                    responseCommand.setCommand("REQUEST-PrivateCHatroom");
                    String sendBack = gson.toJson(responseCommand);
                    byte[] toSendBytes = sendBack.getBytes();
                    int toSendLen = toSendBytes.length;
                    byte[] toSendLenBytes = new byte[4];
                    toSendLenBytes[0] = (byte)(toSendLen & 0xff);
                    toSendLenBytes[1] = (byte)((toSendLen >> 8) & 0xff);
                    toSendLenBytes[2] = (byte)((toSendLen >> 16) & 0xff);
                    toSendLenBytes[3] = (byte)((toSendLen >> 24) & 0xff);
                    byte[] responseAsBytes = sendBack.getBytes();
                    outputStream.write(toSendLenBytes);
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);
                }
                else if(requestCommand.getCommand().equals("DELETE-User")){
                   // System.out.println(requestCommand.getSpecificOrder());
                    userDAO.deleteAccount(requestCommand.getSpecificOrder());
                    userDAO.deleteFriendFromUsers(requestCommand.getSpecificOrder());
                    chatroomDAO.deleteUserFromChatrooms(requestCommand.getSpecificOrder());

                }
                else if(requestCommand.getCommand().equals("UserUpdate"))
                {
                    Account account = gson.fromJson(requestCommand.getSpecificOrder(),Account.class);
                    //System.out.println(account);
                    userDAO.EditAccount(account);
                }
                else if(requestCommand.getCommand().equals("AddTopicToUser"))
                {
                   // System.out.println("Topic added " + requestCommand.getSpecificOrder());
                   // System.out.println("User "  + requestCommand.getVariableUser());
                    userDAO.addTopicToUser(requestCommand.getSpecificOrder(),requestCommand.getVariableUser());

                }
                else if(requestCommand.getCommand().equals("removeTopicFromUser"))
                {
                    //System.out.println("Topic removed " + requestCommand.getSpecificOrder());
                   // System.out.println("User "  + requestCommand.getVariableUser());
                    userDAO.removeUserTopic(requestCommand.getSpecificOrder(),requestCommand.getVariableUser());

                }
                else if (requestCommand.getCommand().equals("ChatroomsByTopic"))
                {
                    String response = gson.toJson(chatroomDAO.getChatroomsByTopic(requestCommand.getSpecificOrder()));
                    responseCommand.setSpecificOrder(response);
                   // System.out.println(response);
                    responseCommand.setCommand("ChatroomsByTopic");
                    String sendBack = gson.toJson(responseCommand);
                    byte[] toSendBytes = sendBack.getBytes();
                    int toSendLen = toSendBytes.length;
                    byte[] toSendLenBytes = new byte[4];
                    toSendLenBytes[0] = (byte)(toSendLen & 0xff);
                    toSendLenBytes[1] = (byte)((toSendLen >> 8) & 0xff);
                    toSendLenBytes[2] = (byte)((toSendLen >> 16) & 0xff);
                    toSendLenBytes[3] = (byte)((toSendLen >> 24) & 0xff);
                    byte[] responseAsBytes = sendBack.getBytes();
                    outputStream.write(toSendLenBytes);
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);
                }
                else if(requestCommand.getCommand().equals("DELETE-PrivateChatroom")){
                    chatroomDAO.deletePrivateChatroom(requestCommand.getSpecificOrder(),requestCommand.getVariableUser());
                }
            }
        } catch (IOException e)
        {
            e.printStackTrace();
        }

    }
}
