using MongoDB.Bson;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Models
{
    public class User
    {
        public string _id { get; set; }
        public string Username { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }

        public User()
        {
            
        }
        public User(string _id, string username, string fname, string lname)
        {
            this._id = _id;
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