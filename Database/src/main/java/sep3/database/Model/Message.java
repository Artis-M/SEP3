package sep3.database.Model;

import org.bson.types.ObjectId;

public class Message {
    private String _id;
    private String message;
    private String authorID;

    public Message(String message, String authorID,String _id) {
        this.message = message;
        this.authorID = authorID;
        this._id=_id;
    }

    public String getMessage() {
        return message;
    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
    }

    public void setAuthorID(String authorID) {
        this.authorID = authorID;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    @Override
    public String toString() {
        return "Message{" +
                "message='" + message + '\'' +
                ", AuthorID=" + authorID +
                '}';
    }

    public String getAuthorID() {
        return authorID;
    }
}
