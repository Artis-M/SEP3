package sep3.database.Persistance;

import org.bson.types.ObjectId;
import sep3.database.Model.Topic;
import sep3.database.Model.TopicList;

import java.util.ArrayList;

public interface TopicDAO {
    void addTopic(Topic topic);
    Topic getTopic(ObjectId id);
    ArrayList<Topic> getUserTopics(ObjectId userId);
    Topic getTopic(String topic);
}
