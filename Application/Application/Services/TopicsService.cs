using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services
{
    public class TopicsService : ITopicsService
    {
        public List<Topic> Topics { get; set; }
        public Model Model;

        public TopicsService(Model modelManager)
        {
            this.Topics = new List<Topic>();
            this.Model = modelManager;
        }

        public async Task AddTopic(Topic topic)
        {
            Topics.Add(topic);
        }

        public async Task RemoveTopic(Topic topic)
        {
            Topics.Remove(topic);
            await Model.DeleteTopic(topic._id);
        }

        public async Task<IList<Topic>> GetAllTopics()
        {
            return Topics;
        }

        public async Task<Topic> GetTopicById(string id)
        {
            foreach (var VARIABLE in Topics)
            {
                if (VARIABLE._id.Equals(id))
                {
                    return VARIABLE;
                }
            }

            return null;
        }
    }
}