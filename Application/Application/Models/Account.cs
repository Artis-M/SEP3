using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.Models
{
    public class Account : User
    {
        public string role { get; set; }
        [JsonPropertyName("pass")]
        public string Pass { get; set; }
        public string email { get; set; }
        public List<User> friends { get; set; }
        public List<Topic> topics { get; set; }

        public Account()
        {
        }

        public Account(string role, string pass, string email, string id, string username, string fname, string lname) :
            base(id, username, fname, lname)
        {
            this.email = email;
            this.Pass = pass;
            this.role = role;
        }

        public Account(string role, string pass, string email, string username, string fname, string lname) : base(
            username, fname, lname)
        {
            this.email = email;
            this.Pass = pass;
            this.role = role;
        }
    }
}