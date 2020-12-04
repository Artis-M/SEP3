package sep3.database.Mediator;


import com.google.gson.Gson;
import org.bson.types.ObjectId;
import sep3.database.Model.Account;
import sep3.database.Persistance.ChatroomDAO;
import sep3.database.Persistance.UserDAO;
import sep3.database.Persistance.UserDAOImpl;

import javax.net.ssl.SSLServerSocketFactory;
import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;

public class ServiceController implements Runnable
{
    private int PORT = 1234;
    private boolean running;
    private ServerSocket welcomeSocket;

    private BufferedReader in;
    private PrintWriter out;
    private Gson gson;
    private ChatroomDAO chatroomDAO;
    private UserDAO userDAO;

    public ServiceController() throws IOException {
        gson = new Gson();
        this.running = true;
        running = true;
        welcomeSocket = new ServerSocket(PORT);
        userDAO = new UserDAOImpl();
    }


    @Override
    public void run() {


        try {
            ServerSocket welcomeSocket = new ServerSocket(2910);
            System.out.println("Server started..");
            Socket socketToClient = welcomeSocket.accept();
            System.out.println("Client connected..");
            InputStream inputStream = socketToClient.getInputStream();
            OutputStream outputStream = socketToClient.getOutputStream();

            while(running)
            {
                try {
                    System.out.println("1");
                    byte[] lenBytes = new byte[4];
                    System.out.println("2");
                    inputStream.readAllBytes();
                  //  inputStream.read(lenBytes, 0, 4);
                    System.out.println("3");
                    int len = (((lenBytes[3] & 0xff) << 24) | ((lenBytes[2] & 0xff) << 16) |
                            ((lenBytes[1] & 0xff) << 8) | (lenBytes[0] & 0xff));
                    byte[] receivedBytes = new byte[len];
                    inputStream.read(receivedBytes, 0, len);
                    System.out.println("Read");

                    String receivedFromClient = new String(receivedBytes, 0, len);
                    CommandLine line = gson.fromJson(receivedFromClient,CommandLine.class);

                if(line.getCommand().equals("REQUEST-User"))
                {
                    System.out.println("Here");
                    CommandLine send = new CommandLine();
                    send.setCommand("UserCredentials");
                    Account account = userDAO.getAccount(line.getJson());
                    String sendJson = gson.toJson(send);
                    byte[] toSendBytes = sendJson.getBytes();
                    int toSendLen = toSendBytes.length;
                    byte[] toSendLenBytes = new byte[4];
                    toSendLenBytes[0] = (byte) (toSendLen & 0xff);
                    toSendLenBytes[1] = (byte) ((toSendLen >> 8) & 0xff);
                    toSendLenBytes[2] = (byte) ((toSendLen >> 16) & 0xff);
                    toSendLenBytes[3] = (byte) ((toSendLen >> 24) & 0xff);
                    outputStream.write(toSendLenBytes);
                    outputStream.write(toSendBytes);

                }

                }catch (Exception e)
                {
                    System.out.println(e.getMessage());
                }

            }
        } catch (IOException e) {
            e.printStackTrace();
        }

    }
}
