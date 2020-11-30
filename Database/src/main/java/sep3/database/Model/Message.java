package sep3.database.Model;

import org.bson.types.ObjectId;

public class Message {
    private ObjectId id;
    private String message;
    private int authorID;

    public Message(String message, int authorID) {
        this.message = message;
        this.authorID = authorID;
    }

    public ObjectId getId() {
        return id;
    }
    public String getMessage() {
        return message;
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
