package sep3.database.Mediator;


import com.google.gson.Gson;
import org.bson.types.ObjectId;
import sep3.database.Model.Account;
import sep3.database.Persistance.ChatroomDAO;
import sep3.database.Persistance.UserDAO;
import sep3.database.Persistance.UserDAOImpl;

import javax.net.ssl.SSLServerSocketFactory;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
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
    private ChatroomDAO chatroomDAO;
    private UserDAO userDAO;

    public ServiceController() throws IOException {
        gson = new Gson();
        this.running = true;
        running = true;
        welcomeSocket = (SSLServerSocketFactory.getDefault()).createServerSocket(PORT);
        userDAO = new UserDAOImpl();
    }


    @Override
    public void run() {


        try {
            System.out.println("ServerSocket is ready for connection...");
            Socket socket = welcomeSocket.accept();
            in = new BufferedReader(
                    new InputStreamReader(socket.getInputStream()));
            out = new PrintWriter(socket.getOutputStream(), true);
            System.out.println("Client Connected");
            while(running)
            {
                String json = in.readLine();
                CommandLine request = gson.fromJson(json,CommandLine.class);
                if(request.getCommand().equals("REQUEST-User"))
                {
                    System.out.println("Here");
                    CommandLine send = new CommandLine();
                    send.setCommand("UserCredentials");
                    Account account = userDAO.getAccount(request.getVariableUser());
                    send.setSpecificOrder(gson.toJson(account));
                    send.setVariableChatroom("");
                    send.setVariableUser("");
                    String sendJson = gson.toJson(send);
                    out.write(sendJson);

                }
               else  if(request.getCommand().equals(""))
                {

                }

            }
        } catch (IOException e) {
            e.printStackTrace();
        }

    }
}
