package sep3.database.Persistance;

import com.google.gson.Gson;
import com.mongodb.BasicDBObject;
import com.mongodb.DBCursor;
import com.mongodb.client.FindIterable;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoCursor;
import org.bson.Document;
import org.bson.types.ObjectId;
import sep3.database.Model.Account;
import sep3.database.Model.User;
import sep3.database.Model.UserList;

import static com.mongodb.client.model.Projections.include;

public class UserDAOImpl implements UserDAO{
    private MongoCollection<Document> collection;
    private DBConnection connection;
    Gson gson;
    public UserDAOImpl()
    {
        connection = DBConnection.setConnection();
        collection = connection.getDatabase().getCollection("Users");
        gson = new Gson();
    }

    public MongoCursor<Document> cursor(String key,Object obj)
    {
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append(key,obj);
        return collection.find(whereQuery).iterator();
    }

    @Override
    public Account getAccount(ObjectId userId) {
        MongoCursor<Document> cursor = cursor("_id",userId);
        String json = cursor.next().toJson();
        return gson.fromJson(json,Account.class);
    }

    @Override
    public Account getAccount(String username) {
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append("username",username);
        MongoCursor<Document> cursor = collection.find(whereQuery).iterator();
        String json = cursor.next().toJson();
        return gson.fromJson(json,Account.class);
    }

    @Override
    public User getUser(ObjectId userID) {
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append("_id",userID);
        MongoCursor<Document> cursor = collection.find(whereQuery).iterator();
        String json = cursor.next().toJson();
        return gson.fromJson(json,User.class);
    }

    @Override
    public UserList getUserFriends(ObjectId userId) {
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append("_id",userId);
        FindIterable findIterable = collection.find(whereQuery).projection(include("friends"));
        System.out.println(findIterable.cursor().next());
        return null;
    }

    @Override
    public void addFriend(User user) {

    }

    @Override
    public void removeUser(User user) {

    }

    @Override
    public void addTopicToUser(String Topic, ObjectId userId) {

    }

    @Override
    public void removeUserTopic(String Topic, ObjectId userId) {

    }

    @Override
    public void addAccount(Account account) {

    }

    @Override
    public Account getUserByName(String firstName, String LastName) {
        return null;
    }
}
