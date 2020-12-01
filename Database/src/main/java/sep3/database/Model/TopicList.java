package sep3.database.Model;

import java.util.ArrayList;

public class TopicList {
    private ArrayList<Topic> topics;

    public TopicList() {
        topics = new ArrayList<>();
    }

    public TopicList(ArrayList<Topic> topics) {
        this.topics = topics;
    }
    public void addTopic(Topic topic)
    {
        topics.add(topic);
    }
    public void removeTopic(Topic topic)
    {
        topics.remove(topic);
    }

    @Override
    public String toString() {
        return "TopicList{" +
                "topics=" + topics +
                '}';
    }

    public ArrayList<Topic> getTopics() {
        return topics;
    }

    public void setTopics(ArrayList<Topic> topics) {
        this.topics = topics;
    }
}
