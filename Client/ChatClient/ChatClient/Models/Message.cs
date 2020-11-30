namespace Models
{
    public class Message
    {
        public string message { get; set; }
        public int Id { get; set; }
        
        public int authorID { get; set; }
    }
}