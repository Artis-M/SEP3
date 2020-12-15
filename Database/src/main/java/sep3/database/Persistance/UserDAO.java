package sep3.database.Persistance;

import org.bson.types.ObjectId;
import sep3.database.Model.Account;
import sep3.database.Model.User;
import sep3.database.Model.UserList;

import java.util.ArrayList;

public interface UserDAO {
    Account getAccount(ObjectId userId);
    Account getAccount(String username);
    User getUser(String userID);
    ArrayList<User> getUserFriends(String userId);
    void addFriend(String friend,String userId);
    void removeFriend(String user,String friendId);
    void addTopicToUser(String Topic,String userId);
    void removeUserTopic(String Topic,String userId);
    void addAccount(Account account);
    ArrayList<Account> getAllAccount();
    void deleteFriendFromUsers(String friend);
    void deleteAccount(String userID);
    void EditAccount(Account account);

}
