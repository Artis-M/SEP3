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
import java.util.Arrays;

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
        Document add = new Document();
        ObjectId _id = new ObjectId(chatroom.get_id());
        add.append("_id",_id);
        add.append("name",chatroom.getName());
        if(chatroom.getTopics().size()!=0) {
            Document topics = new Document();
            for (var topic : chatroom.getTopics()
            ) {
                ObjectId topicID = new ObjectId(topic.get_id());
                topics.append("$set", new BasicDBObject().append("topics", topicID));
            }
            add.append("topics", Arrays.asList(topics));
        }
        if(chatroom.getParticipants().size()!=0) {
            Document friends = new Document();
            for (var friend : chatroom.getParticipants()
            ) {
                ObjectId friendId = new ObjectId(friend.get_id());
                friends.append("$set", new BasicDBObject().append("participants", friendId));
            }
            add.append("participants",Arrays.asList(friends));
        }
        if(chatroom.getMessages().size()!=0) {
            Document messages = new Document();
            for (var message : chatroom.getMessages()
            ) {
                BasicDBObject DBmessage = new BasicDBObject();
                ObjectId authorId = new ObjectId(message.getAuthorID());
                ObjectId messageId = new ObjectId(message.getMessage());
                DBmessage.append("AuthorId",authorId);
                DBmessage.append("messageId",messageId);
                DBmessage.append("message",message.getMessage());
                messages.append("$set", new BasicDBObject().append("messages", DBmessage));
            }
            add.append("participants",Arrays.asList(messages));
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
        }catch (Exception e)
        {
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
        while(documents.hasNext())
        {
            String json = documents.next().toJson();
            Chatroom room = gson.fromJson(json,Chatroom.class);
            chatRooms.add(room);
        }
        return chatRooms;
    }
}
