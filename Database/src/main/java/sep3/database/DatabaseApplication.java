package sep3.database;


import org.bson.types.ObjectId;
import sep3.database.Mediator.ServiceController;
import sep3.database.Model.Account;
import sep3.database.Model.Chatroom;
import sep3.database.Model.Message;
import sep3.database.Model.User;
import sep3.database.Persistance.ChatroomDAO;
import sep3.database.Persistance.ChatroomDAOImpl;
import sep3.database.Persistance.UserDAO;
import sep3.database.Persistance.UserDAOImpl;

import java.io.IOException;
import java.util.ArrayList;

public class DatabaseApplication
{

    public static void main(String[] args) throws IOException
    {
//        ServiceController controller = new ServiceController();
//        Thread t1 = new Thread(controller);
//        t1.start();
        ChatroomDAO dao = new ChatroomDAOImpl();
        ArrayList<Chatroom> chatrooms = dao.getAllChatrooms();
        for (var chatroom: chatrooms
             ) {
            System.out.println(chatrooms);
        }
    }

}
