using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    
    public class LoginServiceImp : LoginService
    {
        private string uri = "https://localhost:5004";
        public async Task<string> Login(string username, string password)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            
            SHA384CryptoServiceProvider sha = new SHA384CryptoServiceProvider();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            byte[] hashedBytes = sha.ComputeHash(passwordBytes);
            string hashedPassword = Convert.ToBase64String(hashedBytes);

            return hashedPassword;
        }

        public Task Register(string username, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}