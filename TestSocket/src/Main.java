import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.ServerSocket;
import java.net.Socket;

public class Main {
    public static void main(String[] args) throws IOException, IOException {

        ServerSocket server = new ServerSocket(3330);
        System.out.println("Server started..");

        Socket socket = server.accept();
        System.out.println("Client connected");

        // read from client
        InputStream inputStream = socket.getInputStream();
        byte[] lenbytes = new byte[1024];
        int read = inputStream.read(lenbytes,0,lenbytes.length);
        String message = new String(lenbytes,0,read);
        System.out.println("Received from client: " + message);


        // respond to client
        OutputStream outputStream = socket.getOutputStream();
        String response = "Hello from server";
        byte[] responseAsBytes = response.getBytes();
        outputStream.write(responseAsBytes, 0, responseAsBytes.length);
        System.out.println("Done");
        socket.close();
    }
}
