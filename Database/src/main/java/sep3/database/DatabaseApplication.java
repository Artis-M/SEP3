package sep3.database;


import org.bson.types.ObjectId;
import sep3.database.Mediator.ServiceController;
import sep3.database.Model.Account;
import sep3.database.Model.User;
import sep3.database.Persistance.UserDAO;
import sep3.database.Persistance.UserDAOImpl;

import java.io.IOException;

public class DatabaseApplication {

    public static void main(String[] args) throws IOException {
//        ServiceController controller = new ServiceController();
//        Thread t1 = new Thread(controller);
//        t1.start();
        UserDAO userDAO = new UserDAOImpl();
        ObjectId query1 = new ObjectId("5fc61e753be97f142192ac87");
        User friend = new User(query1,"usernew23","No name","No name");
        userDAO.removeFriend(friend,new ObjectId("5fc61dd93be97f142192ac85"));
    }

}
