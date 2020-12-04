namespace Application.Models
{
    public class Account : User
    {
        public string role { get; set; }
        public string password { get; set; }
        public string email { get; set; }

        public Account()
        {
        }

        public Account(string role, string pass, string email, string id, string username, string fname, string lname) :
            base(id, username, fname, lname)
        {
            this.email = email;
            this.password = pass;
            this.role = role;
        }

        public Account(string role, string pass, string email, string username, string fname, string lname) : base(
            username, fname, lname)
        {
            this.email = email;
            this.password = pass;
            this.role = role;
        }
    }
}