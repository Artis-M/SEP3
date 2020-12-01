package sep3.database.Model;

import java.util.ArrayList;

public class UserList {

    private ArrayList<User> users;

    public UserList() {
        users = new ArrayList<>();
    }

    public UserList(ArrayList<User> users) {
        this.users = users;
    }
    public void addUser(User user)
    {
        users.add(user);
    }
    public void removeUser(User user)
    {
        users.remove(user);
    }

    public ArrayList<User> getUsers() {
        return users;
    }

    public void setUsers(ArrayList<User> users) {
        this.users = users;
    }
}
