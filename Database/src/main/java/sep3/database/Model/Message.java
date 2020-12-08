package sep3.database.Model;

import org.bson.types.ObjectId;

public class Message {
    private String messageId;
    private String message;
    private String authorID;

    public Message(String message, String authorID,String _id) {
        this.message = message;
        this.authorID = authorID;
        this.messageId=_id;
    }

    public String getMessage() {
        return message;
    }

    public String get_id() {
        return messageId;
    }

    public void set_id(String _id) {
        this.messageId = _id;
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
                "messageId='" + messageId + '\'' +
                ", message='" + message + '\'' +
                ", authorID='" + authorID + '\'' +
                '}';
    }

    public String getAuthorID() {
        return authorID;
    }
}
