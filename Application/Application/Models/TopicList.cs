using System.Collections.Generic;

namespace Application.Models
{
    public class TopicList
    {
        public List<Topic> Topics { get; set; }

        public TopicList(List<Topic> topics)
        {
            this.Topics = topics;
        }

        public TopicList()
        {
            this.Topics = new List<Topic>();
        }

        public void addTopic(Topic topic)
        {
            Topics.Add(topic);
        }

        public void removeTopic(Topic topic)
        {
            Topics.Remove(topic);
        }
    }
}