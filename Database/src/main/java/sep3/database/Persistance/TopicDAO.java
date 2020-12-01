package sep3.database.Persistance;

import org.bson.types.ObjectId;
import sep3.database.Model.Topic;
import sep3.database.Model.TopicList;

public interface TopicDAO {
    void addTopic(Topic topic);
    Topic getTopic(ObjectId id);
    TopicList getUserTopics(ObjectId userId);
    Topic getTopic(String topic);
}
