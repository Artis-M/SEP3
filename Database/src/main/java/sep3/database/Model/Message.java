package sep3.database.Model;

import org.bson.types.ObjectId;

public class Message {
    private ObjectId _id;
    private String message;
    private int authorID;

    public Message(String message, int authorID,ObjectId _id) {
        this.message = message;
        this.authorID = authorID;
        this._id=_id;
    }

    public String getMessage() {
        return message;
    }

    public ObjectId get_id() {
        return _id;
    }

    public void set_id(ObjectId _id) {
        this._id = _id;
    }

    public void setAuthorID(int authorID) {
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

    public int getAuthorID() {
        return authorID;
    }
}
