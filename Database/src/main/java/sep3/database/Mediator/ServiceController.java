package sep3.database.Mediator;


import com.google.gson.Gson;

import javax.net.ssl.SSLServerSocketFactory;
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

    private BufferedReader in;
    private PrintWriter out;
    private Gson gson;

    public ServiceController() throws IOException {
        gson = new Gson();
        this.running = true;
        System.setProperty("javax.net.ssl.keyStore","chatsep.store");
       System.setProperty("javax.net.ssl.keyStorePassword","password");
        running = true;
        welcomeSocket = (SSLServerSocketFactory.getDefault()).createServerSocket(PORT);
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
