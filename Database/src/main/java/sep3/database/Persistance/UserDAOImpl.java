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

/**
 * Class that implement UserDAO interface
 */
public class UserDAOImpl implements UserDAO
{
    private MongoCollection<Document> collection;
    private DBConnection connection;
    private Gson gson;
    private TopicDAO topicDAO;


    /**
     * Parameterless constructor that initiate the connection to Users collection
     */
    public UserDAOImpl()
    {
        connection = DBConnection.setConnection();
        collection = connection.getDatabase().getCollection("Users");
        gson = new Gson();
        topicDAO = new TopicDAOImpl();

    }


    /**
     * Private method that create a Document based on a query
     * @param key name of field in database
     * @param obj what to search
     * @return Document from database
     */
    private MongoCursor<Document> cursor(String key, Object obj)
    {
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append(key, obj);
        return collection.find(whereQuery).iterator();
    }


    /**
     * Create an account based on a document from database
     * @param document document from database
     * @return Account
     */
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

    /**
     * Get an account based on id of a user
     * @param userId id of user
     * @return Account
     */
    @Override
    public Account getAccount(ObjectId userId)
    {
        MongoCursor<Document> cursor = cursor("_id", userId);
        var document = cursor.next();
        return createAccount(document);
    }

    /**
     * Get an account based on username of an account
     * @param username account username
     * @return Account
     */
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

    /**
     * Get all accounts from database
     * @return list of accounts
     */
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


    /**
     * Delete a specific friend from all users
     * @param friend id of user that should be deleted
     */
    @Override
    public void deleteFriendFromUsers(String friend) {
        BasicDBObject friendQuery = new BasicDBObject();

        ObjectId _id = new ObjectId(friend);
        friendQuery.append("$pull", new BasicDBObject().append("friends",_id));


        collection.updateMany(new BasicDBObject(),friendQuery);
    }

    /**
     * Delete an account from database
     * @param userID account id
     */
    @Override
    public void deleteAccount(String userID) {
        ObjectId _id = new ObjectId(userID);
        BasicDBObject delete = new BasicDBObject();
        delete.append("_id",_id);
        collection.deleteOne(delete);
    }

    /**
     * Edit an existing account from database
     * @param account edited account
     */
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


    /**
     * Get User based on id
     * @param userID user id
     * @return User
     */
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


    /**
     * Get all friend for a specific account based on id
     * @param userId account id
     * @return list of users
     */
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

    /**
     * Add friend to an existing account
     * @param friend friend id
     * @param userId account id
     */
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

    /**
     * remove friend from a specific account
     * @param user account id
     * @param friendId friend id
     */
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


    /**
     * Add topic to an Account based on topic name
     * @param Topic topic name
     * @param userId account id
     */
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

    /**
     * remove topic from an account
     * @param Topic topic name
     * @param userId account id
     */
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

    /**
     * Add an account to the database
     * @param account account to add
     */
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

        collection.insertOne(add);
    }


}
