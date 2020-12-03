using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Model;

namespace Application.Services
{
    public class TopicsService : ITopicsService
    {
        public List<Topic> Topics { get; set; }

        public TopicsService()
        {
            this.Topics = new List<Topic>();
        }

        public async Task AddTopic(Topic topic)
        {
            Topics.Add(topic);
        }

        public async Task RemoveTopic(Topic topic)
        {
            Topics.Remove(topic);
        }

        public async Task<IList<Topic>> GetAllTopics()
        {
            return Topics;
        }

        public async Task<Topic> GetTopicById(string id)
        {
            foreach (var VARIABLE in Topics)
            {
                if (VARIABLE.ID.Equals(id))
                {
                    return VARIABLE;
                }
            }

            return null;
        }
    }
}