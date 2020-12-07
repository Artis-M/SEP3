﻿namespace Application.Models
{
    public class Topic
    {
        public string _id { get; set; }
        public string name { get; set; }

        public Topic(string id, string name)
        {
            this.name = name;
            this._id = id;
        }

        public Topic(string name)
        {
            this.name = name;
        }

        public Topic()
        {
        }
    }
}