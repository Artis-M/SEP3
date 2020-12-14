package sep3.database;
import sep3.database.Mediator.ServiceController;
import sep3.database.Persistance.ChatroomDAO;
import sep3.database.Persistance.ChatroomDAOImpl;

import java.io.IOException;

public class DatabaseApplication
{

    public static void main(String[] args) throws IOException
    {
//        ServiceController controller = new ServiceController();
//        Thread t1 = new Thread(controller);
//        t1.start();
        ChatroomDAO dao = new ChatroomDAOImpl();
        dao.deletePrivateChatroom("5fd527aed5725937f8346daa","5fd33f3cb6128c817e9b1896");
    }

}
