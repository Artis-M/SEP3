using System.Collections.Generic;
using Microsoft.AspNetCore.Connections;

namespace Application.Model
{
    public class UserList
    {
        public List<User> Users { get; set; }

        public UserList()
        {
            this.Users = new List<User>();
        }

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public void RemoveUser(User user)
        {
            Users.Remove(user);
        }
    }
}