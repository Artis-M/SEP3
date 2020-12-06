package sep3.database.Persistance;

import sep3.database.Model.Chatroom;

import java.util.ArrayList;

public interface ChatroomDAO {
    ArrayList<Chatroom> getAllChatrooms();
    void AddChatroom (Chatroom chatroom);
    void deleteChatroom(Chatroom chatroom);
    void updateChatroom(Chatroom chatroom);
    void getChatroom(String name);
}
