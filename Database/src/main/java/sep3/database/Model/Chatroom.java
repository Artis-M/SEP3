package sep3.database.Model;

import java.util.ArrayList;

public class Chatroom {
    private String _id;
    private String name;
    private ArrayList<User> participants;
    private ArrayList<Message> messages;
    private ArrayList<Topic> topics;


    public Chatroom() {
        participants = new ArrayList<>();
        messages = new ArrayList<>();
        topics = new ArrayList<>();
    }

    public Chatroom(String _id, String name, ArrayList<User> participants, ArrayList<Message> messages, ArrayList<Topic> topics) {
        this._id = _id;
        this.name = name;
        this.participants = participants;
        this.messages = messages;
        this.topics = topics;
    }

    public Chatroom(String _id, String name, ArrayList<User> participants, ArrayList<Topic> topics) {
        this._id = _id;
        this.name = name;
        this.participants = participants;
        this.topics = topics;
    }

    public Chatroom(String _id, String name, ArrayList<User> participants) {
        this._id = _id;
        this.name = name;
        this.participants = participants;
    }
    public void removeUser(User user)
    {
        participants.remove(user);
    }
    public void addUser(User user)
    {
        participants.add(user);
    }
    public void addMessage(Message message)
    {
        messages.add(message);
    }
    public void addTopic(Topic topic)
    {
        topics.add(topic);
    }
    public void removeTopic(Topic topic)
    {
        topics.remove(topic);
    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public ArrayList<User> getParticipants() {
        return participants;
    }

    public void setParticipants(ArrayList<User> participants) {
        this.participants = participants;
    }

    public ArrayList<Message> getMessages() {
        return messages;
    }

    public void setMessages(ArrayList<Message> messages) {
        this.messages = messages;
    }

    public ArrayList<Topic> getTopics() {
        return topics;
    }

    public void setTopics(ArrayList<Topic> topics) {
        this.topics = topics;
    }

    @Override
    public String toString() {
        return "Chatroom{" +
                "_id='" + _id + '\'' +
                ", name='" + name + '\'' +
                ", participants=" + participants +
                ", messages=" + messages +
                ", topics=" + topics +
                '}';
    }
}
