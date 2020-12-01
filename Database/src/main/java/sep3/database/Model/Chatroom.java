package sep3.database.Model;

import org.bson.types.ObjectId;

import java.util.ArrayList;

public class Chatroom {
    private ObjectId _id;
    private String name;
    private UserList participants;
    private MessageList messages;
    private TopicList topics;

    public Chatroom(ObjectId _id, String name, UserList participants, MessageList messages, TopicList topics) {
        this._id = _id;
        this.name = name;
        this.participants = participants;
        this.messages = messages;
        this.topics = topics;
    }

    public Chatroom(ObjectId _id, String name, UserList participants, MessageList messages) {
        this._id = _id;
        this.name = name;
        this.participants = participants;
        this.messages = messages;
    }

    public Chatroom(ObjectId _id, String name, UserList participants, TopicList topics) {
        this._id = _id;
        this.name = name;
        this.participants = participants;
        this.topics = topics;
    }

    public Chatroom(ObjectId _id, String name, UserList participants) {
        this._id = _id;
        this.name = name;
        this.participants = participants;
    }
    public void removeUser(User user)
    {
        participants.removeUser(user);
    }
    public void addUser(User user)
    {
        participants.addUser(user);
    }
    public void addMessage(Message message)
    {
        messages.addMessage(message);
    }
    public void addTopic(Topic topic)
    {
        topics.addTopic(topic);
    }
    public void removeTopic(Topic topic)
    {
        topics.removeTopic(topic);
    }

    public ObjectId get_id() {
        return _id;
    }

    public void set_id(ObjectId _id) {
        this._id = _id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public UserList getParticipants() {
        return participants;
    }

    public void setParticipants(UserList participants) {
        this.participants = participants;
    }

    public MessageList getMessages() {
        return messages;
    }

    public void setMessages(MessageList messages) {
        this.messages = messages;
    }

    public TopicList getTopics() {
        return topics;
    }

    public void setTopics(TopicList topics) {
        this.topics = topics;
    }
}
