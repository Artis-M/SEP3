package sep3.database.Mediator;


import com.google.gson.Gson;
import org.springframework.ui.Model;
import sep3.database.Persistance.MessageDAO;
import sep3.database.Persistance.PersistanceInterface;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;

public class ServiceController implements Runnable
{
    private int PORT = 1234;
    private boolean running;
    private ServerSocket welcomeSocket;
    private MessageDAO DAO;
    private Socket socket;
    private BufferedReader in;
    private PrintWriter out;
    private Gson gson;

    public ServiceController() throws IOException {
        DAO = new MessageDAO();
        gson = new Gson();
        this.running = true;
        this.socket = socket;
        in = new BufferedReader(
                new InputStreamReader(this.socket.getInputStream()));
        out = new PrintWriter(this.socket.getOutputStream(), true);
    }

    @Override
    public void run() {

        try {
            running = true;
            welcomeSocket = new ServerSocket(PORT);
            Socket socket = welcomeSocket.accept();
            while(running)
            {
                String request = in.readLine();
                String json = in.readLine();
                if(request.equals(""))
                {

                }

            }
        } catch (IOException e) {
            e.printStackTrace();
        }

    }
}
