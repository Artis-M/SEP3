
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class User
    {
        public string _id { get; set; }
        [Required,MinLength(3,ErrorMessage = "Please Enter Username with at least 3 characters")]
        [JsonPropertyName("Username")]
        public string Username { get; set; }
        [Required,MinLength(1,ErrorMessage = "Please Enter First Name")]
        [JsonPropertyName("Fname")]
        public string Fname { get; set; }
        [Required,MinLength(1,ErrorMessage = "Please Enter Last Name")]
        [JsonPropertyName("Lname")]
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