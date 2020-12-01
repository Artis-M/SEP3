package sep3.database.Model;

import java.util.ArrayList;

public class MessageList {
    private ArrayList<Message> messages;
    public MessageList()
    {
        this.messages = new ArrayList<>();

    }

    public MessageList(ArrayList<Message> messages) {
        this.messages = messages;
    }
    public void addMessage(Message message)
    {
        messages.add(message);
    }
    public void removeMessage(Message message)
    {
        messages.remove(message);
    }
    public ArrayList<Message> getMessages(){
        return messages;
    }
}
