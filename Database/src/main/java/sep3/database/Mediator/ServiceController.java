package sep3.database.Mediator;


import com.google.gson.Gson;
import sep3.database.Model.Account;
import sep3.database.Persistance.UserDAO;
import sep3.database.Persistance.UserDAOImpl;

import javax.net.ssl.SSLServerSocketFactory;
import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;

public class ServiceController implements Runnable
{
    private int PORT = 8443;
    private boolean running;
    private ServerSocket welcomeSocket;

    private BufferedReader in;
    private PrintWriter out;
    private Gson gson;
    private UserDAO userDAO;

    public ServiceController() throws IOException {
        gson = new Gson();
        this.running = true;
        running = true;
        userDAO = new UserDAOImpl();
        welcomeSocket  = new ServerSocket(PORT);
    }


    @Override
    public void run() {


        try {
            System.out.println("ServerSocket is ready for connection...");
            Socket socket = welcomeSocket.accept();
            InputStream inputStream = socket.getInputStream();

            System.out.println("Client Connected");
            while(running)
            {
                byte[] lenbytes = new byte[1024];
                int read = inputStream.read(lenbytes,0,lenbytes.length);
                String request = new String(lenbytes,0,read);
                CommandLine line = gson.fromJson(request,CommandLine.class);
                System.out.println("Received from client: " + request);

                OutputStream outputStream = socket.getOutputStream();
                if(request.equals("REQUEST-User"))
                {
                   Account account =  userDAO.getAccount(line.getVariableUser());
                   String accountToJson = gson.toJson(account);
                   CommandLine sendLine = new CommandLine();
                   sendLine.setCommand("REQUEST-User");
                   sendLine.setVariableUser(accountToJson);
                   String send = gson.toJson(sendLine);
                    byte[] responseAsBytes = send.getBytes();
                    outputStream.write(responseAsBytes, 0, responseAsBytes.length);

                }

            }
        } catch (IOException e) {
            e.printStackTrace();
        }

    }
}
