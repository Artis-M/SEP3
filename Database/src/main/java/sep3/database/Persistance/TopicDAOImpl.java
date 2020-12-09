package sep3.database.Persistance;

import com.google.gson.Gson;
import com.mongodb.BasicDBObject;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoCursor;
import org.bson.Document;
import org.bson.types.ObjectId;
import sep3.database.Model.Topic;
import sep3.database.Model.TopicList;

import java.util.ArrayList;
import java.util.NoSuchElementException;
import java.util.Objects;

public class TopicDAOImpl implements TopicDAO {
    private MongoCollection<Document> collection;
    private DBConnection connection;
    private Gson gson;
    public TopicDAOImpl()
    {
        connection = DBConnection.setConnection();
        collection = connection.getDatabase().getCollection("Topics");
        gson = new Gson();
    }
    private MongoCursor<Document> cursor(String key, Object obj) {
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append(key, obj);
        return collection.find(whereQuery).iterator();
    }
    public void addTopic(Topic topic)
    {
        collection = connection.getDatabase().getCollection("Topics");
        Document document = new Document();
        ObjectId _id = new ObjectId(topic.get_id());
        document.append("_id",_id);
        document.append("name",topic.getName());
        collection.insertOne(document);


    }

    public Topic getTopic(ObjectId id)
    {
        collection = connection.getDatabase().getCollection("Topics");
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append("_id",id);
        Document document = (Objects.requireNonNull(collection.find(whereQuery).first()));
        Topic topic = new Topic(document.get("_id").toString(), document.getString("name"));
        return topic;
    }
    public ArrayList<Topic> getUserTopics(ObjectId userId)
    {


        collection = connection.getDatabase().getCollection("Users");
        TopicList topicList = new TopicList();
        MongoCursor<Document> cursor = cursor("_id",userId);
        Document document;
        try {
            document = cursor.next();
        }
        catch(NoSuchElementException e)
        {
            return null;
        }
        var topics = document.getList("topics", ObjectId.class);
        if(topics!=null) {
            for (ObjectId id : topics
            ) {
                System.out.println("Here");
                Topic topic = getTopic(id);
                System.out.println(topic);
                topicList.addTopic(topic);
            }
        }

        return topicList.getTopics();
    }

    @Override
    public Topic getTopic(String topic) {
        collection = connection.getDatabase().getCollection("Topics");
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append("name",topic);
        var document = collection.find(whereQuery).first();
        System.out.println(document);
        if(document==null)
        {
            ObjectId id = new ObjectId();

            addTopic(new Topic(id.toString(),topic));
            document = collection.find(whereQuery).first();
        }
        ObjectId id = new ObjectId(document.get("_id").toString());
        Topic topic1 = new Topic(id.toString(),document.get("name").toString());
        System.out.println(topic1);
        return topic1;
    }
}
