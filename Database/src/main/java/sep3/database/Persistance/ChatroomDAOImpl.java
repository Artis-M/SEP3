package sep3.database.Persistance;

import com.google.gson.Gson;
import com.mongodb.BasicDBObject;
import com.mongodb.DBObject;
import com.mongodb.client.FindIterable;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoCursor;
import com.mongodb.client.model.Filters;
import org.bson.Document;
import org.bson.conversions.Bson;
import org.bson.types.ObjectId;
import sep3.database.Model.Chatroom;
import sep3.database.Model.Message;
import sep3.database.Model.Topic;
import sep3.database.Model.User;

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

    /**
     * parameterless constructor that initiate the connection
     */

    public ChatroomDAOImpl() {
        connection = DBConnection.setConnection();
        collection = connection.getDatabase().getCollection("Chatrooms");
        gson = new Gson();
        userDAO = new UserDAOImpl();
        topicDAO = new TopicDAOImpl();
    }

    /**
     * Getting a document from database and create chatroom
     * @param document
     * @return chatroom
     */
    private Chatroom createChatroom(Document document) {
        Chatroom room = new Chatroom();
        String id = document.get("_id").toString();
        room.set_id(id);
        room.setName(document.get("name").toString());
        room.setOwner(document.get("owner").toString());
        room.setType(document.get("type").toString());
        List<Document> messages = (List<Document>) document.get("messages");
        if(messages!=null) {

            for (var DBmessage : messages
            ) {
                Message message = new Message(
                        DBmessage.get("message").toString(),
                        DBmessage.get("AuthorId").toString(),
                        DBmessage.get("messageId").toString(),DBmessage.get("Username").toString());
                room.addMessage(message);
            }
        }

        ArrayList<User> participants = getChatroomUsers(id);
        room.setParticipants(participants);
        ArrayList<Topic> topics = getChatroomTopic(id);
        room.setTopics(topics);
        return room;
    }

    /**
     * get topics of chatroom based on id as string
     * @param id id of a topic
     * @return list of chatrooms
     */
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

        }
        return topicList;
    }


    /**
     * Get All users of a specific chatroom
     * @param id id of chatroom
     * @return list of Users
     */
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
            User participant = userDAO.getUser(participantId.toString());
            users.add(participant);


        }
        return users;
    }

    /**
     * Get all chatrooms from database
     * @return list of chatrooms
     */
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

    /**
     * Add a chatroom in the database
     * @param chatroom chatroom to add
     */
    @Override
    public void AddChatroom(Chatroom chatroom) {
        Document add = new Document();
        ObjectId _id = new ObjectId(chatroom.get_id());
        add.append("_id", _id);
        add.append("name", chatroom.getName());
        add.append("owner",new ObjectId(chatroom.getOwner()));
        add.append("type",chatroom.getType());

        if (chatroom.getTopics()!=null) {
            Document topics = new Document();
            for (var topic : chatroom.getTopics()
            ) {
                String topic_id = topicDAO.getTopic(topic.getName()).get_id();
                ObjectId topicID = new ObjectId(topic_id);
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
                DBmessage.append("Username", message.getUsername());
                DBmessage.append("message", message.getMessage());
                messages.add(DBmessage);
            }
            add.append("messages", messages);
        }
        collection.insertOne(add);

    }


    /**
     * Add a message to a chatroom
     * @param chatroomId id of specific chatroom
     * @param message message information to add
     */
    @Override
    public void addMessageToChatroom(String chatroomId, Message message) {
        BasicDBObject chatroomObject = new BasicDBObject();
        BasicDBObject updateMessage = new BasicDBObject();
        BasicDBObject messageObject = new BasicDBObject();

        ObjectId authorId = new ObjectId(message.getAuthorID());
        ObjectId messageId = new ObjectId();
        messageObject.append("AuthorId", authorId);
        messageObject.append("messageId", messageId);
        messageObject.append("message", message.getMessage());
        messageObject.append("Username", message.getUsername());

        updateMessage.append("$push", new BasicDBObject().append("messages",messageObject));
        ObjectId chatroom_id = new ObjectId(chatroomId);
        chatroomObject.append("_id", chatroom_id);
        collection.updateOne(chatroomObject,updateMessage);
    }


    /**
     * Add a user to a chatroom
     * @param userId id of user
     * @param chatroomId id of chatroom
     */
    @Override
    public void joinChatroom(String userId, String chatroomId) {
        BasicDBObject chatroomObject = new BasicDBObject();
        BasicDBObject updatePList = new BasicDBObject();
        BasicDBObject participantObject = new BasicDBObject();

        ObjectId participantId = new ObjectId(userId);
        participantObject.append("participantId", participantId);
        System.out.println("JOINNNNNNNNNNNNNNNNNNNNNNNNNNNNN");

        updatePList.append("$push", new BasicDBObject().append("participants",participantObject));
        ObjectId chatroom_id = new ObjectId(chatroomId);
        chatroomObject.append("_id", chatroom_id);
        collection.updateOne(chatroomObject,updatePList);

    }


    /**
     * delete user from a chatroom
     * @param userId id of user
     * @param chatroomId id of chatroom
     */
    @Override
    public void leaveChatroom(String userId, String chatroomId) {
        System.out.println(chatroomId);
        BasicDBObject chatroomObject = new BasicDBObject();
        BasicDBObject updatePList = new BasicDBObject();
        BasicDBObject participantObject = new BasicDBObject();
        ObjectId participantId = new ObjectId(userId);
        participantObject.append("participantId", participantId);
        updatePList.append("$pull", new BasicDBObject().append("participants",participantObject));
        ObjectId chatroom_id = new ObjectId(chatroomId);
        chatroomObject.append("_id", chatroom_id);
        collection.updateOne(chatroomObject,updatePList);


    }

    /**
     * Get a private chatroom between two participants
     * @param userId1 one of the participants
     * @param userId2 another one of the participants
     * @return private chatroom
     */
    public Chatroom getPrivateChatroom(String userId1,String userId2)
    {
        BasicDBObject object = new BasicDBObject();
        ObjectId user1 = new ObjectId(userId1);
        object.append("participantId",user1);
        BasicDBObject object1 = new BasicDBObject();
        ObjectId user2 = new ObjectId(userId2);
        object1.append("participantId",user2);
        Bson users = Filters.and(Filters.eq("participants",object),
                Filters.eq("participants",object1),
                Filters.eq("type","private")
        );
        Document document = collection.find(users).first();

        assert document != null;
        return createChatroom(document);

    }

    /**
     * Get a chatroom based on a id
     * @param id id of chatroom
     * @return chatroom
     */
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

    /**
     * Delete a chatroom from database
     * @param id id of chatroom
     */
    @Override
    public void removeChatroom(String id) {
        ObjectId _id = new ObjectId(id);
        BasicDBObject remove = new BasicDBObject("_id",_id);
        collection.deleteOne(remove);
    }

    /**
     * Delete a private chatroom based on participants from database
     * @param userId1 first participant
     * @param userId2  second participant
     */
    @Override
    public void deletePrivateChatroom(String userId1, String userId2) {
        BasicDBObject object = new BasicDBObject();
        ObjectId user1 = new ObjectId(userId1);
        object.append("participantId",user1);
        BasicDBObject object1 = new BasicDBObject();
        ObjectId user2 = new ObjectId(userId2);
        object1.append("participantId",user2);
        Bson users = Filters.and(Filters.eq("participants",object),
                Filters.eq("participants",object1),
                Filters.eq("type","private")
        );
        collection.deleteOne(users);
    }


    /**
     * Get chatrooms where a specific user is participant
     * @param userId id of user
     * @return list of chatrooms
     */
    @Override
    public ArrayList<Chatroom> getChatroomByUserId(String userId) {
        ArrayList<Chatroom> chatRooms = new ArrayList<>();
        BasicDBObject whereQuery = new BasicDBObject();
        BasicDBObject participant = new BasicDBObject();

        ObjectId _id = new ObjectId(userId);
        participant.append("participantId",_id);
        whereQuery.append("participants", participant);
        whereQuery.append("type","public");
        MongoCursor<Document> documents = collection.find(whereQuery).iterator();


        while (documents.hasNext()) {
            Document json = documents.next();
            Chatroom room = createChatroom(json);
            chatRooms.add(room);
        }
        return chatRooms;
    }

    /**
     * Delete user from all chatrooms where it is as participant from database
     * @param userId id of user
     */
    @Override
    public void deleteUserFromChatrooms(String userId) {
        BasicDBObject whereQuery = new BasicDBObject();
        BasicDBObject participant = new BasicDBObject();
        ObjectId _id = new ObjectId(userId);
        participant.append("participantId",_id);
        whereQuery.append("$pull", new BasicDBObject().append("participants",participant));
        collection.updateMany(new BasicDBObject(),whereQuery);
    }


    /**
     * Get chatrooms by topic name
     * @param topic name of topic
     * @return list of chatrooms
     */
    @Override
    public ArrayList<Chatroom> getChatroomsByTopic(String topic) {
        ArrayList<Chatroom> chatRooms = new ArrayList<>();
        BasicDBObject whereQuery = new BasicDBObject();
        BasicDBObject participant = new BasicDBObject();

        Topic topicObject = topicDAO.getTopic(topic.toLowerCase());
        participant.append("topics",new ObjectId(topicObject.get_id()));
        whereQuery.append("topics", participant);
        MongoCursor<Document> documents = collection.find(whereQuery).iterator();
        while (documents.hasNext()) {
            Document json = documents.next();
            Chatroom room = createChatroom(json);
            chatRooms.add(room);
        }
        System.out.println(chatRooms.size());
        return chatRooms;
    }
}
