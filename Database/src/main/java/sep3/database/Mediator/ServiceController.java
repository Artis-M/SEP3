package sep3.database.Mediator;


import com.google.gson.Gson;

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

    public ServiceController() throws IOException {
        gson = new Gson();
        this.running = true;
        running = true;
        //removed ssl
        //welcomeSocket = (SSLServerSocketFactory.getDefault()).createServerSocket(PORT);
        welcomeSocket  = new ServerSocket(PORT);
    }


    @Override
    public void run() {


        try {
            System.out.println("ServerSocket is ready for connection...");
            Socket socket = welcomeSocket.accept();
            InputStream inputStream = socket.getInputStream();


            out = new PrintWriter(socket.getOutputStream(), true);
            System.out.println("Client Connected");
            while(running)
            {
                byte[] lenbytes = new byte[1024];
                int read = inputStream.read(lenbytes,0,lenbytes.length);
                String request = new String(lenbytes,0,read);

                System.out.println("Received from client: " + request);

                if(request.equals(""))
                {

                }

            }
        } catch (IOException e) {
            e.printStackTrace();
        }

    }
}
