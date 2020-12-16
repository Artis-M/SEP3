using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Account : User
    {
        public string role { get; set; }

        [JsonPropertyName("Pass")]
        [Required, MinLength(6, ErrorMessage = "Please Enter Password")]
        public string Pass { get; set; }

        [Required] public string email { get; set; }
        public List<User> friends { get; set; }
        public List<Topic> topics { get; set; }
    }
}