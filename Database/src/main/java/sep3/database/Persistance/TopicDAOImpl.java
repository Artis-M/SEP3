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
        document.append("_id",topic.get_id());
        document.append("name",topic.getName());
        collection.insertOne(document);


    }
    public Topic getTopic(ObjectId id)
    {
        collection = connection.getDatabase().getCollection("Topics");
        BasicDBObject whereQuery = new BasicDBObject();
        whereQuery.append("_id",id);
        String document = (Objects.requireNonNull(collection.find(whereQuery).first())).toJson();
        Topic topic = gson.fromJson(document,Topic.class);
        return topic;
    }
    public ArrayList<Topic> getUserTopics(ObjectId userId)
    {
        collection = connection.getDatabase().getCollection("Users");
        TopicList topicList = new TopicList();
        MongoCursor<Document> cursor = cursor("_id",userId);
        var document = cursor.next();
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
        if(document==null)
        {
            ObjectId id = new ObjectId();

            addTopic(new Topic(id,topic));
            document = collection.find(whereQuery).first();
        }
        ObjectId id = new ObjectId(document.get("_id").toString());
        Topic topic1 = new Topic(id,document.get("name").toString());
        return topic1;
    }
}
