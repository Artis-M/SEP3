package sep3.database.Mediator;


import com.google.gson.Gson;
import sep3.database.Model.Account;
import sep3.database.Model.Chatroom;
import sep3.database.Model.Message;
import sep3.database.Model.User;
import sep3.database.Persistance.*;

import javax.net.ssl.SSLServerSocketFactory;
import java.io.*;
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
    private BufferedReader in;
    private PrintWriter out;
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

            out = new PrintWriter(socket.getOutputStream(), true);
            System.out.println("Client Connected");
            while (running)
            {
                byte[] lenbytes = new byte[1024];
                int read = inputStream.read(lenbytes, 0, lenbytes.length);
                String request = new String(lenbytes, 0, read);


                CommandLine requestCommand = gson.fromJson(request, CommandLine.class);
                CommandLine responseCommand = new CommandLine();
                if (requestCommand.getCommand().equals("REQUEST-UserCredentials"))
                {

                    CommandLine commandLine1 = new CommandLine();
                    String response = gson.toJson(userDAO.getAllAccount());
                    commandLine1.setSpecificOrder(response);
                    commandLine1.setCommand("UserCredentials");
                    String sendBack = gson.toJson(commandLine1);
                    byte[] responseAsBytes = sendBack.getBytes();
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);

                } else if (requestCommand.getCommand().equals("REQUEST-User"))
                {

                    String response = gson.toJson(userDAO.getAccount(requestCommand.getVariableUser()));
                    responseCommand.setSpecificOrder(response);
                    responseCommand.setCommand("OneUserCredential");
                    String sendBack = gson.toJson(responseCommand);
                    byte[] responseAsBytes = sendBack.getBytes();
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);

                } else if (requestCommand.getCommand().equals("REQUEST-UserByID"))
                {

                    String response = gson.toJson(userDAO.getUser(requestCommand.getVariableUser()));
                    responseCommand.setSpecificOrder(response);
                    responseCommand.setCommand("OneUserCredentialByID");
                    String sendBack = gson.toJson(responseCommand);
                    byte[] responseAsBytes = sendBack.getBytes();
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
                    byte[] responseAsBytes = sendBack.getBytes();
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);

                } else if (requestCommand.getCommand().equals("REQUEST-ChatroomByUser"))
                {


                    String response = gson.toJson(chatroomDAO.getChatroomByUserId(requestCommand.getVariableUser()));
                    responseCommand.setSpecificOrder(response);
                    responseCommand.setCommand("ChatroomByUser");
                    String sendBack = gson.toJson(responseCommand);
                    byte[] responseAsBytes = sendBack.getBytes();
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);

                } else if (requestCommand.getCommand().equals("JOIN-Chatroom"))
                {

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
                    ArrayList<User> specificOrder = gson.fromJson(requestCommand.getSpecificOrder(),ArrayList.class);
                    User user1 = specificOrder.get(0);
                    User user2 = specificOrder.get(1);
                    userDAO.addFriend(user1.get_id(), user2.get_id());
                    userDAO.addFriend(user2.get_id(),user1.get_id());
                }
                else if(requestCommand.getCommand().equals("ChatroomNew")){

                    Chatroom chatroom = gson.fromJson(requestCommand.getSpecificOrder(),Chatroom.class);
                    System.out.println();
                    chatroomDAO.AddChatroom(chatroom);
                }
            }
        } catch (IOException e)
        {
            e.printStackTrace();
        }

    }
}
