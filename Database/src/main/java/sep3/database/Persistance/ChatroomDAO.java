package sep3.database.Persistance;

import sep3.database.Model.Chatroom;
import sep3.database.Model.Message;

import java.util.ArrayList;

public interface ChatroomDAO {
    ArrayList<Chatroom> getAllChatrooms();
    void AddChatroom (Chatroom chatroom);
    void addMessageToChatroom(String chatroomId, Message message);
    void joinChatroom(String userId,String chatroomId);
    void leaveChatroom(String userId,String chatroomId);
    Chatroom getChatroom(String id);
    void removeChatroom(String id);
    void deletePrivateChatroom(String userId1,String userId2);
    ArrayList<Chatroom> getChatroomByUserId(String userId);
    void deleteUserFromChatrooms(String userId);
    ArrayList<Chatroom> getChatroomsByTopic(String topic);
    Chatroom getPrivateChatroom(String userId1,String userId2);
}
