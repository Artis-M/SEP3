﻿using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.Text.Json.Serialization;
 using Models;

 namespace Application.Models
{
    public class Account : User
    {
        public string role { get; set; }
       [JsonPropertyName("Pass")]
       [Required,MinLength(6,ErrorMessage = "Please Enter Password")]
        public string Pass { get; set; }
        [Required]
        public string email { get; set; }
        public List<User> friends { get; set; }
        public List<Topic> topics { get; set; }

        public Account()
        {
        }

        public Account(string role, string pass, string email, string id, string username, string fname, string lname, string PictureURL) :
            base(id, username, fname, lname,PictureURL)
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