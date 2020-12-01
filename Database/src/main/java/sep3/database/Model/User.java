package sep3.database.Model;

import org.bson.types.ObjectId;

public class User {
    private ObjectId _id;
    private String Username;
    private String Fname;
    private String Lname;


    public User(ObjectId _id, String username, String fname, String lname) {
        this._id = _id;
        Username = username;
        Fname = fname;
        Lname = lname;
    }

    public User(String username, String fname, String lname) {
        Username = username;
        Fname = fname;
        Lname = lname;
    }

    public ObjectId get_id() {
        return _id;
    }

    public void set_id(ObjectId _id) {
        this._id = _id;
    }

    public String getUsername() {
        return Username;
    }

    public void setUsername(String username) {
        Username = username;
    }

    public String getFname() {
        return Fname;
    }

    public void setFname(String fname) {
        Fname = fname;
    }

    public String getLname() {
        return Lname;
    }

    public void setLname(String lname) {
        Lname = lname;
    }

    @Override
    public String toString() {
        return "User{" +
                "_id=" + _id +
                ", Username='" + Username + '\'' +
                ", Fname='" + Fname + '\'' +
                ", Lname='" + Lname + '\'' +
                '}';
    }
}
