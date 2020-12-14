package sep3.database.Model;

import java.util.ArrayList;

public class Account extends User{
    private String role;
    private String Pass;
    private String email;
    private ArrayList<User> friends;
    private ArrayList<Topic> topics;


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

    public Account(String role, String Pass, String _id, String username, String fname, String lname, String email,String PictureURL) {
        super(_id, username, fname, lname,PictureURL);
        friends = new ArrayList<>();
        this.topics = new ArrayList<>();
        this.email=email;
        this.role=role;
        this.Pass =Pass;
    }

    public Account(String role,String Pass,String username, String fname, String lname,String email) {
        super(username, fname, lname);
        this.role=role;
        this.Pass =Pass;
        this.email=email;
        friends = new ArrayList<>();
        this.topics = new ArrayList<>();

    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public ArrayList<User> getFriends() {
        return friends;
    }

    public void setFriends(ArrayList<User> friends) {
        this.friends = friends;
    }

    public ArrayList<Topic> getTopics() {
        return topics;
    }

    public void setTopics(ArrayList<Topic> topics) {
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
        topics.add(topic);
    }

    public void addFriend(User friend)
    {
        friends.add(friend);
    }

    public void setPass(String pass) {
        this.Pass = pass;
    }

}
