using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Models
{
    public class User
    {
        public string _id { get; set; }

        [Required, MinLength(3, ErrorMessage = "Please Enter Username with at least 3 characters")]
        [JsonPropertyName("Username")]
        public string Username { get; set; }

        [Required, MinLength(1, ErrorMessage = "Please Enter First Name")]
        [JsonPropertyName("Fname")]
        public string Fname { get; set; }

        [Required, MinLength(1, ErrorMessage = "Please Enter Last Name")]
        [JsonPropertyName("Lname")]
        public string Lname { get; set; }

        public string PictureURL { get; set; }

        public User()
        {
        }

        public User(string _id, string username, string fname, string lname, string pictureUrl)
        {
            this._id = _id;
            this.Username = username;
            this.Fname = fname;
            this.Lname = lname;
            this.PictureURL = pictureUrl;
        }

        public User(string username, string fname, string lname)
        {
            this.Username = username;
            this.Fname = fname;
            this.Lname = lname;
        }
    }
}