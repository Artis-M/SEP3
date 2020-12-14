package sep3.database.Model;

import org.bson.types.ObjectId;

public class User {
    private String _id;
    private String Username;
    private String Fname;
    private String Lname;
    private String PictureURL;


    public User(String _id, String username, String fname, String lname, String PictureURL) {
        this._id = _id;
        Username = username;
        Fname = fname;
        Lname = lname;
        this.PictureURL = PictureURL;
    }

    public User(String username, String fname, String lname) {
        Username = username;
        Fname = fname;
        Lname = lname;
    }

    public String get_id() {
        return _id;
    }

    public void set_id(ObjectId _id) {
        this._id = _id.toString();
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

    public String getPictureURL()
    {
        return PictureURL;
    }

    public void setPictureURL(String pictureURL)
    {
        PictureURL = pictureURL;
    }
}
