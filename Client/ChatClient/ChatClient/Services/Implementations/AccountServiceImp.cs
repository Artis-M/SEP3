using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Models;
using Models;

namespace Services
{
    
    public class AccountService : IAccountService
    {
        private string uri = "https://localhost:5004/accounts/";
        public async Task<Account> Login(string username, string password)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            
            SHA384CryptoServiceProvider sha = new SHA384CryptoServiceProvider();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            byte[] hashedBytes = sha.ComputeHash(passwordBytes);
            string hashedPassword = Convert.ToBase64String(hashedBytes);
            Console.WriteLine($"HashedPass:{hashedPassword}");

            string request = $"login/?username={username}&password={hashedPassword}";
            Console.WriteLine(uri+request);
                
           HttpResponseMessage responseMessage = await http.GetAsync(request);
           Console.WriteLine(await responseMessage.Content.ReadAsStringAsync());
           if (responseMessage.StatusCode == HttpStatusCode.OK)
           {
               Account account = JsonSerializer.Deserialize<Account>(await responseMessage.Content.ReadAsStringAsync());
               return account;
           }
           else
           {
               throw new Exception("Incorrect username or password!");
           }
        }

        public async Task Register(Account account)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            string serialized = JsonSerializer.Serialize(account);
            StringContent content = new StringContent(serialized,Encoding.UTF8,"application/json");

            HttpResponseMessage responseMessage = await http.PostAsync(uri+"register", content);
            Console.WriteLine(responseMessage.ToString());
        }
    }
}