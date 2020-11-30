package sep3.database.Persistance;

import org.springframework.stereotype.Service;
import sep3.database.Model.Message;

import java.util.ArrayList;

public interface PersistanceInterface {
    ArrayList<Message> getAllMessages();
    void addMessage(Message message);
}
