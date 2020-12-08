using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Models
{
    public class User
    {
        public string _id { get; set; }
        [Required,MinLength(8,ErrorMessage = "Please Enter Username with at least 8 characters")]
        public string Username { get; set; }
        [Required,MinLength(1,ErrorMessage = "Please Enter First Name")]
        public string Fname { get; set; }
        [Required,MinLength(1,ErrorMessage = "Please Enter Last Name")]
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