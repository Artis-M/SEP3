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
import java.util.List;
import java.util.NoSuchElementException;
import java.util.Objects;

public class TopicDAOImpl implements TopicDAO {
    private MongoCollection<Document> collection;
    private DBConnection connection;
    private Gson gson;

    /**
     * Constructor initiate connection to collection Topics
     */
    public TopicDAOImpl()
    {
        connection = DBConnection.setConnection();
        collection = connection.getDatabase().getCollection("Topics");
        gson = new Gson();
    }

    /**
     * Private method that create a Document based on a query
     * @param key name of field in database
     * @param obj what to search
     * @return Document from database
     */
    private MongoCursor<Document> cursor(String key, Object obj) {
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append(key, obj);
        return collection.find(whereQuery).iterator();
    }

    /**
     * Add topic to the database
     * @param topic
     */
    public void addTopic(Topic topic)
    {
        collection = connection.getDatabase().getCollection("Topics");
        Document document = new Document();
        ObjectId _id = new ObjectId(topic.get_id());
        document.append("_id",_id);
        document.append("name",topic.getName().toLowerCase());
        collection.insertOne(document);


    }

    /**
     * Get topic based on id
     * @param id id of topic
     * @return topic
     */
    public Topic getTopic(ObjectId id)
    {
        collection = connection.getDatabase().getCollection("Topics");
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append("_id",id);
        Document document = (Objects.requireNonNull(collection.find(whereQuery).first()));
        Topic topic = new Topic(document.get("_id").toString(), document.getString("name"));
        return topic;
    }

    /**
     * Get all topics for a user based on id of user
     * @param userId id of user
     * @return list of topics
     */
    public ArrayList<Topic> getUserTopics(ObjectId userId)
    {


        collection = connection.getDatabase().getCollection("Users");
        ArrayList<Topic> topicList = new ArrayList();
        MongoCursor<Document> cursor = cursor("_id",userId);
        Document document;
        List<ObjectId> topics = null;
        try {

            document = cursor.next();
            System.out.println(document);
            topics = document.getList("topics", ObjectId.class);
            System.out.println(topics);
        }
        catch(Exception e)
        {
            System.out.println("Exception");

        }


        if(topics!=null) {
            System.out.println("Not null");
            for (var id : topics
            ) {

                Topic topic = getTopic(id);
                topicList.add(topic);
            }

        }

        return topicList;
    }

    /**
     * Get topic based on topic name
     * @param topic topic name
     * @return Topic
     */
    @Override
    public Topic getTopic(String topic) {
        collection = connection.getDatabase().getCollection("Topics");
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append("name",topic.toLowerCase());
        var document = collection.find(whereQuery).first();
        if(document==null)
        {
            ObjectId id = new ObjectId();
            addTopic(new Topic(id.toString(),topic));
            document = collection.find(whereQuery).first();
        }
        ObjectId id = new ObjectId(document.get("_id").toString());
        Topic topic1 = new Topic(id.toString(),document.get("name").toString());
        return topic1;
    }
}
