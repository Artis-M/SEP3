package sep3.database.Persistance;

import com.google.gson.Gson;
import com.mongodb.BasicDBObject;
import com.mongodb.client.FindIterable;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoCursor;
import org.bson.Document;
import org.bson.types.ObjectId;
import sep3.database.Model.*;

import java.util.ArrayList;

import static com.mongodb.client.model.Projections.include;

public class ChatroomDAOImpl implements ChatroomDAO{
    private MongoCollection<Document> collection;
    private DBConnection connection;
    private Gson gson;
    private UserDAO userDAO;
    private TopicDAO topicDAO;

    public ChatroomDAOImpl() {
        connection = DBConnection.setConnection();
        collection = connection.getDatabase().getCollection("Chatrooms");
        gson = new Gson();
        userDAO = new UserDAOImpl();
        topicDAO = new TopicDAOImpl();
    }

    private Chatroom createChatroom(Document document)
    {
        Chatroom room = new Chatroom();
        String id = document.get("_id").toString();
        room.set_id(id);
        room.setName(document.get("name").toString());
        ArrayList<Message> messages = (ArrayList<Message>)document.getList("messages", Message.class);
        room.setMessages(messages);
        ArrayList<User> participants = getChatroomUsers(id);
        room.setParticipants(participants);
        ArrayList<Topic> topics = getChatroomTopic(id);
        room.setTopics(topics);

        return room;
    }

    private ArrayList<Topic> getChatroomTopic(String id)
    {
        ArrayList<Topic> topics = new ArrayList<>();
        BasicDBObject whereQuery = new BasicDBObject();
        ObjectId _id = new ObjectId(id);
        whereQuery.append("_id", _id);
        FindIterable<Document> findIterable = collection.find(whereQuery).projection(include("topics"));
        var document = findIterable.cursor().next();
        var chatTopics = document.getList("topics", ObjectId.class);
        if (chatTopics != null) {
            for (ObjectId topicID : chatTopics
            ) {
                Topic topic = topicDAO.getTopic(_id);
                topics.add(topic);
            }
        }
        return topics;
    }


    private ArrayList<User> getChatroomUsers(String id)
    {
        ArrayList<User> users = new ArrayList<>();
        BasicDBObject whereQuery = new BasicDBObject();
        ObjectId _id = new ObjectId(id);
        whereQuery.append("_id", _id);
        FindIterable<Document> findIterable = collection.find(whereQuery).projection(include("participants"));
        var document = findIterable.cursor().next();
        var participants = document.getList("participants", ObjectId.class);
        if (participants != null) {
            for (ObjectId participantId : participants
            ) {
                User participant = userDAO.getUser(_id);
                users.add(participant);
            }
        }
        return users;
    }

    @Override
    public ArrayList<Chatroom> getAllChatrooms() {
        ArrayList<Chatroom> chatrooms = new ArrayList<>();
        MongoCursor<Document> cursor = collection.find().iterator();
        try {
            while (cursor.hasNext()) {

                Document document = cursor.next();
                Chatroom chatroom = createChatroom(document);
                chatrooms.add(chatroom);
            }
        } finally {
            cursor.close();
        }
        return chatrooms;
    }

    @Override
    public void AddChatroom(Chatroom chatroom) {

    }

    @Override
    public void deleteChatroom(Chatroom chatroom) {

    }

    @Override
    public void updateChatroom(Chatroom chatroom) {

    }

    @Override
    public void getChatroom(String name) {

    }
}
