namespace Application.Models
{
    public class Topic
    {
        public string ID { get; set; }
        public string name { get; set; }

        public Topic(string id, string name)
        {
            this.name = name;
            this.ID = id;
        }

        public Topic(string name)
        {
            this.name = name;
        }
        
    }
}