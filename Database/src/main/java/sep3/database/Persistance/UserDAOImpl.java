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
import java.util.NoSuchElementException;

import static com.mongodb.client.model.Projections.exclude;
import static com.mongodb.client.model.Projections.include;

public class UserDAOImpl implements UserDAO
{
    private MongoCollection<Document> collection;
    private DBConnection connection;
    private Gson gson;
    private TopicDAO topicDAO;


    public UserDAOImpl()
    {
        connection = DBConnection.setConnection();
        collection = connection.getDatabase().getCollection("Users");
        gson = new Gson();
        topicDAO = new TopicDAOImpl();

    }

    private MongoCursor<Document> cursor(String key, Object obj)
    {
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append(key, obj);
        return collection.find(whereQuery).iterator();
    }

    public Account createAccount(Document document)
    {
        ObjectId _id = new ObjectId(document.get("_id").toString());
        Account account = new Account(
                document.get("role").toString(), document.get("Pass").toString()
                , _id.toString(), document.get("Username").toString(),
                document.get("Fname").toString(), document.get("Lname").toString(), document.get("email").toString(),document.get("PictureURL").toString()
        );
        account.setTopics(topicDAO.getUserTopics(_id));
        System.out.println(account.getTopics());
        account.setFriends(getUserFriends(_id.toString()));
        return account;

    }

    @Override
    public Account getAccount(ObjectId userId)
    {
        MongoCursor<Document> cursor = cursor("_id", userId);
        var document = cursor.next();
        return createAccount(document);
    }

    @Override
    public Account getAccount(String username)
    {
        MongoCursor<Document> cursor = cursor("Username", username);
        Document document = null;
        try
        {
            document = cursor.next();
        } catch (NoSuchElementException e)
        {
            System.out.println(e.getMessage());
            return null;

        }
        return createAccount(document);
    }

    public ArrayList<Account> getAllAccount()
    {
        ArrayList<Account> accounts = new ArrayList<>();
        MongoCursor<Document> cursor = collection.find().iterator();
        try
        {
            while (cursor.hasNext())
            {

                Document document = cursor.next();
                Account account = createAccount(document);
                accounts.add(account);
            }
        } finally
        {
            cursor.close();
        }
        return accounts;

    }

    @Override
    public void deleteFriendFromUsers(String friend) {
        BasicDBObject friendQuery = new BasicDBObject();

        ObjectId _id = new ObjectId(friend);
        friendQuery.append("$pull", new BasicDBObject().append("friends",_id));


        collection.updateMany(new BasicDBObject(),friendQuery);
    }

    @Override
    public void deleteAccount(String userID) {
        ObjectId _id = new ObjectId(userID);
        BasicDBObject delete = new BasicDBObject();
        delete.append("_id",_id);
        collection.deleteOne(delete);
    }

    @Override
    public void EditAccount(Account account) {
        BasicDBObject edit = new BasicDBObject();
        BasicDBObject toEdit = new BasicDBObject();
        toEdit.append("_id",new ObjectId(account.get_id()));
        edit.append("Username", account.getUsername());
        edit.append("Pass", account.getPass());
        edit.append("Fname", account.getFname());
        edit.append("Lname", account.getLname());
        edit.append("role", account.getRole());
        edit.append("email", account.getEmail());
        BasicDBObject update = new BasicDBObject();
        edit.append("PictureURL",account.getPictureURL());
        update.put("$set",edit);
        collection.updateOne(toEdit,update);
    }

    @Override
    public User getUser(String userID)
    {
        BasicDBObject whereQuery = new BasicDBObject();
        ObjectId objectId = new ObjectId(userID);
        whereQuery.append("_id", objectId);
        MongoCursor<Document> cursor = collection.find(whereQuery).iterator();
        try
        {
            Document document = cursor.next();
            return new User(document.get("_id").toString(), document.get("Username").toString(),
                    document.get("Fname").toString(), document.get("Lname").toString(), document.get("PictureURL").toString());
        } catch (NoSuchElementException e)
        {

        }
        return null;
    }


    @Override
    public ArrayList<User> getUserFriends(String userId)
    {
        ArrayList<User>  list = new ArrayList<>();
        BasicDBObject whereQuery = new BasicDBObject();

        whereQuery.append("_id", new ObjectId(userId));
        FindIterable<Document> findIterable = collection.find(whereQuery).projection(include("friends"));
        Document document;
        try
        {
            document = findIterable.cursor().next();
        } catch (NoSuchElementException e)
        {
            return null;
        }
        var friends = document.getList("friends", ObjectId.class);
        if (friends != null)
        {
            for (ObjectId id : friends
            )
            {
                User friend = getUser(id.toString());
                list.add(friend);
            }
        }
        return list;
    }

    @Override
    public void addFriend(String friend,String userId)
    {
        BasicDBObject newDocument = new BasicDBObject();
        ObjectId friendId = new ObjectId(friend);
        ObjectId currentUser = new ObjectId(userId);
        newDocument.append("$push", new BasicDBObject().append("friends", friendId));
        BasicDBObject searchQuery = new BasicDBObject();
        searchQuery.append("_id", currentUser);
        collection.updateOne(searchQuery, newDocument);


    }

    @Override
    public void removeFriend(String user, String friendId)
    {
        ObjectId friend_id = new ObjectId(friendId);
        BasicDBObject update = new BasicDBObject("friends", friend_id);
        BasicDBObject searchQuery = new BasicDBObject();
        ObjectId userId = new ObjectId(user);
        searchQuery.append("_id", userId);
        System.out.println("REmove " + friendId + " FROM " + user);
        collection.updateOne(searchQuery, new BasicDBObject("$pull", update));
    }

    @Override
    public void addTopicToUser(String Topic, String userId)
    {
        BasicDBObject newDocument = new BasicDBObject();
        ObjectId topicID = new ObjectId(topicDAO.getTopic(Topic).get_id());
        newDocument.append("$push", new BasicDBObject().append("topics",topicID));
        BasicDBObject searchQuery = new BasicDBObject();
        ObjectId user_id = new ObjectId(userId);
        searchQuery.append("_id", user_id);
        collection.updateOne(searchQuery, newDocument);

    }

    @Override
    public void removeUserTopic(String Topic, String userId)
    {
        BasicDBObject newDocument = new BasicDBObject();
        ObjectId topicID = new ObjectId(topicDAO.getTopic(Topic).get_id());
        newDocument.append("$pull", new BasicDBObject().append("topics", topicID));
        BasicDBObject searchQuery = new BasicDBObject();
        ObjectId user_id = new ObjectId(userId);
        searchQuery.append("_id", user_id);
        collection.updateOne(searchQuery, newDocument);
    }

    @Override
    public void addAccount(Account account)
    {


        Document add = new Document();
        ObjectId _id = new ObjectId(account.get_id());
        add.append("_id", _id);
        add.append("Username", account.getUsername());
        add.append("Pass", account.getPass());
        add.append("Fname", account.getFname());
        add.append("Lname", account.getLname());
        add.append("role", account.getRole());
        add.append("email", account.getEmail());
        add.append("PictureURL",account.getPictureURL());
        if (account.getTopics()!= null)
        {
            Document topics = new Document();
            for (var topic : account.getTopics()
            )
            {
                Topic addTopic = topicDAO.getTopic(topic.getName());
                topics.append("$set", new BasicDBObject().append("topics", addTopic.get_id()));
            }
            add.append("topics", Arrays.asList(topics));
        }
        if (account.getFriends().size() != 0)
        {
            Document friends = new Document();
            for (var friend : account.getFriends()
            )
            {
                ObjectId friendId = new ObjectId(friend.get_id());
                friends.append("$set", new BasicDBObject().append("topics", friendId));
            }
            add.append("friends", Arrays.asList(friends));
        }
        collection.insertOne(add);
    }

    @Override
    public Account getUserByName(String firstName, String LastName)
    {
        return null;
    }

}
