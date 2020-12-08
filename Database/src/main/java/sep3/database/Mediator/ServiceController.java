package sep3.database.Mediator;


import com.google.gson.Gson;
import sep3.database.Model.Account;
import sep3.database.Persistance.*;

import javax.net.ssl.SSLServerSocketFactory;
import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;

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

                System.out.println("Received from client: " + request);
                CommandLine requestCommand = gson.fromJson(request, CommandLine.class);
                System.out.println(requestCommand.getCommand());
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
                    System.out.println("Done sending user credentials");
                } else if (requestCommand.getCommand().equals("REQUEST-User"))
                {

                    String response = gson.toJson(userDAO.getAccount(requestCommand.getVariableUser()));
                    responseCommand.setSpecificOrder(response);
                    responseCommand.setCommand("OneUserCredential");
                    String sendBack = gson.toJson(responseCommand);
                    byte[] responseAsBytes = sendBack.getBytes();
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);
                    System.out.println("Done sending user credentials");

                } else if (requestCommand.getCommand().equals("UserNew"))
                {
                    System.out.println("Add new User");
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
                    System.out.println("Done sending chatrooms");
                } else if (requestCommand.getCommand().equals("REQUEST-ChatroomByUser"))
                {

                    System.out.println(responseCommand.getVariableUser());
                    String response = gson.toJson(chatroomDAO.getChatroomByUserId(requestCommand.getVariableUser()));
                    responseCommand.setSpecificOrder(response);
                    responseCommand.setCommand("ChatroomByUser");
                    String sendBack = gson.toJson(responseCommand);
                    byte[] responseAsBytes = sendBack.getBytes();
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);
                    System.out.println("Done sending chatroom for user:" + responseCommand.getVariableUser());
                }
                else if (requestCommand.getCommand().equals("JOIN-Chatroom"))
                {
                    String userId = requestCommand.getVariableUser();
                    String chatroomId = requestCommand.getVariableChatroom();
                    chatroomDAO.joinChatroom(userId,chatroomId);
                }
            }
        } catch (IOException e)
        {
            e.printStackTrace();
        }

    }
}
