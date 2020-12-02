using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Model
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }

        public User(string id, string username, string fname, string lname)
        {
            this.Id = id;
            this.Username = username;
            this.Fname = fname;
            this.Lname = lname;
        }
        public User( string username, string fname, string lname)
        {
         
            this.Username = username;
            this.Fname = fname;
            this.Lname = lname;
        }
    }
}