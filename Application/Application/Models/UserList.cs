using System.Collections.Generic;


namespace Application.Models
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