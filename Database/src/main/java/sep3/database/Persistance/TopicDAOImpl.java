package sep3.database.Persistance;

import com.google.gson.Gson;
import com.mongodb.client.MongoCollection;
import org.bson.Document;
import org.bson.types.ObjectId;
import sep3.database.Model.Topic;

public class TopicDAOImpl implements TopicDAO {
    private MongoCollection<Document> collection;
    private DBConnection connection;
    public TopicDAOImpl()
    {
        connection = DBConnection.setConnection();
        collection = connection.getDatabase().getCollection("Topics");
    }
    public void addTopic(Topic topic)
    {
        Document document = new Document();
        document.append("_id",topic.get_id());
        document.append("topic",topic.getName());
        collection.insertOne(document);


    }
}
