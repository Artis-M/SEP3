package sep3.database.Persistance;

import org.springframework.stereotype.Service;
import sep3.database.Model.Message;

import java.util.ArrayList;

@Service
public class MongoDBManager implements PersistanceInterface {
    private  MessageDAO messageDAO = new MessageDAO();
    @Override
    public ArrayList<Message> getAllMessages() {
        return messageDAO.getMessages();
    }

    @Override
    public void addMessage(Message message) {
    messageDAO.addMessage(message);
    }
}
