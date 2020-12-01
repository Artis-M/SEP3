package sep3.database.Model;

import org.bson.types.ObjectId;

import java.sql.Array;
import java.util.ArrayList;

public class Account extends User{
    private String role;
    private String Pass;
    private String email;
    private UserList friends;
    private TopicList topics;


    @Override
    public String toString() {
        return "Account{" +
                "ID='" + get_id() + '\'' +
                "Username='" + getUsername() + '\'' +
                "FirstName='" + getFname() + '\'' +
                "LastName='" + getLname() + '\'' +
                "role='" + role + '\'' +
                ", Pass='" + Pass + '\'' +
                ", email='" + email + '\'' +
                ", friends=" + friends +
                ", topics=" + topics +

                '}';
    }

    public Account(String role, String Pass, ObjectId _id, String username, String fname, String lname, String email) {
        super(_id, username, fname, lname);
        friends = new UserList();
        this.topics = new TopicList();
        this.email=email;
        this.role=role;
        this.Pass=Pass;
    }

    public Account(String role,String Pass,String username, String fname, String lname,String email) {
        super(username, fname, lname);
        this.role=role;
        this.Pass=Pass;
        this.email=email;
        friends = new UserList();
        this.topics = new TopicList();

    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public UserList getFriends() {
        return friends;
    }

    public void setFriends(UserList friends) {
        this.friends = friends;
    }

    public TopicList getTopics() {
        return topics;
    }

    public void setTopics(TopicList topics) {
        this.topics = topics;
    }

    public String getRole() {
        return role;
    }


    public void setRole(String role) {
        this.role = role;
    }


    public String getPass() {
        return Pass;
    }

    public void addTopic(Topic topic)
    {
        topics.addTopic(topic);
    }

    public void addFriend(User friend)
    {
        friends.addUser(friend);
    }

    public void setPass(String pass) {
        Pass = pass;
    }

}
