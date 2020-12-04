package sep3.database.Persistance;

import org.bson.types.ObjectId;
import sep3.database.Model.Account;
import sep3.database.Model.User;
import sep3.database.Model.UserList;

import java.util.ArrayList;

public interface UserDAO {
    Account getAccount(ObjectId userId);
    Account getAccount(String username);
    User getUser(ObjectId userID);
    ArrayList<User> getUserFriends(ObjectId userId);
    void addFriend(User user,ObjectId userId);
    void removeFriend(User user,ObjectId userId);
    void addTopicToUser(String Topic,ObjectId userId);
    void removeUserTopic(String Topic,ObjectId userId);
    void addAccount(Account account);
    Account getUserByName(String firstName,String LastName);
    ArrayList<Account> getAllAccount();

}
