using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TopicServiceImpl:ITopicService
    {
        private string uri = "https://localhost:5004/topics/";
        HttpClient client = new HttpClient();
        
    }
}