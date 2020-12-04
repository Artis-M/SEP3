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
        ServiceController controller = new ServiceController();
       controller.run();
    }

}
