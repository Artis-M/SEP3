package sep3.database.Persistance;

import com.google.gson.Gson;
import com.mongodb.BasicDBObject;
import com.mongodb.DBObject;
import com.mongodb.client.FindIterable;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoCursor;
import org.bson.Document;
import org.bson.types.ObjectId;
import org.springframework.util.RouteMatcher;
import sep3.database.Model.*;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import static com.mongodb.client.model.Projections.include;

public class ChatroomDAOImpl implements ChatroomDAO {
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

    private Chatroom createChatroom(Document document) {
        System.out.println(document);

        Chatroom room = new Chatroom();
        String id = document.get("_id").toString();
        room.set_id(id);
        room.setName(document.get("name").toString());
        List<Document> messages = (List<Document>) document.get("messages");
        if(messages.size()!=0) {
            for (var DBmessage : messages
            ) {
                Message message = new Message(
                        DBmessage.get("message").toString(),
                        DBmessage.get("AuthorId").toString(), DBmessage.get("messageId").toString());
                room.addMessage(message);

            }
        }

        ArrayList<User> participants = getChatroomUsers(id);
        System.out.println(participants);
        room.setParticipants(participants);
        ArrayList<Topic> topics = getChatroomTopic(id);
        room.setTopics(topics);

        return room;
    }

    private ArrayList<Topic> getChatroomTopic(String id) {
        ArrayList<Topic> topicList = new ArrayList<>();
        BasicDBObject whereQuery = new BasicDBObject();
        ObjectId _id = new ObjectId(id);
        whereQuery.append("_id", _id);
        FindIterable<Document> findIterable = collection.find(whereQuery).projection(include("topics"));
        var document = findIterable.cursor().next();
        try {
            List<Document> topics = (List<Document>) document.get("topics");
            for (var DBtopic : topics
            ) {
                ObjectId topicid = new ObjectId(DBtopic.get("topicId").toString());
                Topic topic = topicDAO.getTopic(topicid);
                topicList.add(topic);


            }
        }catch (NullPointerException e)
        {
            System.out.println(e.getMessage());
        }
        return topicList;
    }


    private ArrayList<User> getChatroomUsers(String id) {
        ArrayList<User> users = new ArrayList<>();
        BasicDBObject whereQuery = new BasicDBObject();
        ObjectId _id = new ObjectId(id);
        whereQuery.append("_id", _id);
        FindIterable<Document> findIterable = collection.find(whereQuery).projection(include("participants"));
        var document = findIterable.cursor().next();

        List<Document> participants = (List<Document>) document.get("participants");
        for (var DBparticipant : participants
        ) {
            ObjectId participantId = new ObjectId(DBparticipant.get("participantId").toString());
            User participant = userDAO.getUser(participantId);
            users.add(participant);


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
        Document add = new Document();
        ObjectId _id = new ObjectId(chatroom.get_id());
        add.append("_id", _id);
        add.append("name", chatroom.getName());

        if (chatroom.getTopics().size() != 0) {
            Document topics = new Document();
            for (var topic : chatroom.getTopics()
            ) {
                ObjectId topicID = new ObjectId(topic.get_id());
                topics.append("topics", topicID);
            }
            add.append("topics", Arrays.asList(topics));
        }
        if (chatroom.getParticipants().size() != 0) {
            List<DBObject> participants = new ArrayList<>();
            for (var friend : chatroom.getParticipants()
            ) {
                ObjectId participantId = new ObjectId(friend.get_id());
                participants.add(new BasicDBObject("participantId", participantId));

            }
            add.append("participants", participants);
        }
        if (chatroom.getMessages().size() != 0) {
            List<DBObject> messages = new ArrayList<>();
            for (var message : chatroom.getMessages()
            ) {
                BasicDBObject DBmessage = new BasicDBObject();
                ObjectId authorId = new ObjectId(message.getAuthorID());
                ObjectId messageId = new ObjectId(message.get_id());
                DBmessage.append("AuthorId", authorId);
                DBmessage.append("messageId", messageId);
                DBmessage.append("message", message.getMessage());
                messages.add(DBmessage);
            }
            add.append("messages", messages);
        }
        collection.insertOne(add);

    }

    @Override
    public void deleteChatroom(Chatroom chatroom) {

    }

    @Override
    public void updateChatroom(Chatroom chatroom) {

    }

    @Override
    public Chatroom getChatroom(String id) {
        BasicDBObject whereQuery = new BasicDBObject();
        ObjectId _id = new ObjectId(id);
        whereQuery.append("_id", _id);
        Chatroom chat = null;
        try {
            Document cursor = collection.find(whereQuery).first();
            String json = cursor.toJson();
            chat = gson.fromJson(json, Chatroom.class);
        } catch (Exception e) {
            return null;
        }

        return chat;

    }

    @Override
    public ArrayList<Chatroom> getChatroomByUserId(String userId) {
        ArrayList<Chatroom> chatRooms = new ArrayList<>();
        BasicDBObject whereQuery = new BasicDBObject();
        ObjectId _id = new ObjectId(userId);
        whereQuery.append("participants", _id);
        MongoCursor<Document> documents = collection.find(whereQuery).iterator();
        while (documents.hasNext()) {
            String json = documents.next().toJson();
            Chatroom room = gson.fromJson(json, Chatroom.class);
            chatRooms.add(room);
        }
        return chatRooms;
    }
}
