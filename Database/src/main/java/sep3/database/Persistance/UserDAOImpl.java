package sep3.database.Persistance;

import com.google.gson.Gson;
import com.mongodb.BasicDBObject;
import com.mongodb.DBCursor;
import com.mongodb.client.FindIterable;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoCursor;
import org.bson.Document;
import org.bson.types.ObjectId;
import sep3.database.Model.*;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;

import static com.mongodb.client.model.Projections.exclude;
import static com.mongodb.client.model.Projections.include;

public class UserDAOImpl implements UserDAO {
    private MongoCollection<Document> collection;
    private DBConnection connection;
    private Gson gson;
    private TopicDAO topicDAO;

    public UserDAOImpl() {
        connection = DBConnection.setConnection();
        collection = connection.getDatabase().getCollection("Users");
        gson = new Gson();
        topicDAO = new TopicDAOImpl();
    }

    private MongoCursor<Document> cursor(String key, Object obj) {
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append(key, obj);
        return collection.find(whereQuery).iterator();
    }

    public Account createAccount(Document document) {
        ObjectId _id = new ObjectId(document.get("_id").toString());
        Account account = new Account(
                document.get("role").toString(), document.get("Pass").toString()
                , _id.toString(), document.get("Username").toString(),
                document.get("Fname").toString(), document.get("Lname").toString(), document.get("email").toString()
        );
        account.setTopics(topicDAO.getUserTopics(_id));
        account.setFriends(getUserFriends(_id));
        return account;

    }

    @Override
    public Account getAccount(ObjectId userId) {
        MongoCursor<Document> cursor = cursor("_id", userId);
        var document = cursor.next();
        return createAccount(document);
    }

    @Override
    public Account getAccount(String username) {
        MongoCursor<Document> cursor = cursor("username", username);
        var document = cursor.next();
        return createAccount(document);
    }

    public ArrayList<Account> getAllAccount()
    {
        ArrayList<Account> accounts = new ArrayList<>();
        MongoCursor<Document> cursor = collection.find().iterator();
        try {
            while (cursor.hasNext()) {

                Document document = cursor.next();
               // System.out.println(json);
                Account account = createAccount(document);
                accounts.add(account);
            }
        } finally {
            cursor.close();
        }
        return accounts;

    }
    @Override
    public User getUser(ObjectId userID) {
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append("_id", userID);
        MongoCursor<Document> cursor = collection.find(whereQuery).iterator();
      //  String json = cursor.next().toJson();
        Document document = cursor.next();
        User user = new User(document.get("_id").toString(),document.get("Username").toString(),
                                document.get("Fname").toString(),document.get("Lname").toString());
        return user;
    }


    @Override
    public ArrayList<User> getUserFriends(ObjectId userId) {
        UserList list = new UserList();
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append("_id", userId);
        FindIterable<Document> findIterable = collection.find(whereQuery).projection(include("friends"));
        var document = findIterable.cursor().next();
        var friends = document.getList("friends", ObjectId.class);
        if (friends != null) {
            for (ObjectId id : friends
            ) {
                User friend = getUser(id);
                list.addUser(friend);
            }
        }
        return list.getUsers();
    }

    @Override
    public void addFriend(User friend,ObjectId userId) {
        BasicDBObject newDocument = new BasicDBObject();
        newDocument.append("$push", new BasicDBObject().append("friends", friend.get_id()));
        BasicDBObject searchQuery = new BasicDBObject();
        searchQuery.append("_id", userId);
        collection.updateOne(searchQuery,newDocument);


    }

    @Override
    public void removeFriend(User user,ObjectId userId) {
        BasicDBObject update = new BasicDBObject("friends",user.get_id());
        BasicDBObject searchQuery = new BasicDBObject();
        searchQuery.append("_id", userId);
        collection.updateOne(searchQuery,new BasicDBObject("$pull",update));
    }

    @Override
    public void addTopicToUser(String Topic, ObjectId userId) {
        BasicDBObject newDocument = new BasicDBObject();
        newDocument.append("$push", new BasicDBObject().append("topics",topicDAO.getTopic(Topic).get_id()));
        BasicDBObject searchQuery = new BasicDBObject();
        searchQuery.append("_id", userId);
        collection.updateOne(searchQuery,newDocument);

    }

    @Override
    public void removeUserTopic(String Topic, ObjectId userId) {
        BasicDBObject update = new BasicDBObject("friends",topicDAO.getTopic(Topic).get_id());
        BasicDBObject searchQuery = new BasicDBObject();
        searchQuery.append("_id", userId);
        collection.updateOne(searchQuery,new BasicDBObject("$pull",update));
    }

    @Override
    public void addAccount(Account account) {


    Document add = new Document();
    add.append("_id",account.get_id());
    add.append("Username",account.getUsername());
    add.append("Pass",account.getPass());
    add.append("Fname",account.getFname());
    add.append("Lname",account.getLname());
    add.append("role",account.getRole());
    add.append("email",account.getEmail());
        if(account.getTopics().size()!=0) {
            Document topics = new Document();
            for (var topic : account.getTopics()
            ) {
                topics.append("$set", new BasicDBObject().append("topics", topic.get_id()));
            }
            add.append("topics", Arrays.asList(topics));
        }
        if(account.getFriends().size()!=0) {
            Document friends = new Document();
            for (var friend : account.getFriends()
            ) {
                friends.append("$set", new BasicDBObject().append("topics", friend.get_id()));
            }
            add.append("friends",Arrays.asList(friends));
        }
    collection.insertOne(add);
    }

    @Override
    public Account getUserByName(String firstName, String LastName) {
        return null;
    }

}
