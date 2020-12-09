using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Application.Models;
using Models;

namespace Services
{
    public class AccountService : IAccountService
    {
        private string uri = "https://localhost:5004/accounts/";
        private readonly IJSRuntime jsRuntime;

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
            string request = $"login";

            http.DefaultRequestHeaders.Add("username", username);
            http.DefaultRequestHeaders.Add("password", hashedPassword);


            HttpResponseMessage responseMessage = await http.GetAsync(request);
            //Console.Out.WriteLine(responseMessage.StatusCode);
            Account account = null;
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                account = JsonSerializer.Deserialize<Account>(await responseMessage.Content.ReadAsStringAsync());
            }

            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("Incorrect username or password!");
            }

            return account;
        }

        public async Task Register(Account account)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            SHA384CryptoServiceProvider sha = new SHA384CryptoServiceProvider();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(account.Pass);
            byte[] hashedBytes = sha.ComputeHash(passwordBytes);
            account.Pass = Convert.ToBase64String(hashedBytes);

            string serialized = JsonSerializer.Serialize(account);
            StringContent content = new StringContent(serialized, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await http.PostAsync(uri + "register", content);
            Console.WriteLine(responseMessage.ToString());
        }

        public async Task AddFriend(string UserID)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            string request = $"addFriend";

            string userJson = await http.GetStringAsync(request);
            User targetUser = JsonSerializer.Deserialize<User>(userJson);

            Account userAccount =
                JsonSerializer.Deserialize<Account>(
                    await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser"));
            userAccount.friends.Add(targetUser);
            User theOneThatsAdding = new User
            {
                _id = userAccount._id,
                Fname = userAccount.Fname,
                Lname = userAccount.Lname,
                Username = userAccount.Username
            };

            await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser",
                JsonSerializer.Serialize(userAccount));
            
            List<User> twoFriends = new List<User>();
            twoFriends.Add(targetUser);
            twoFriends.Add(theOneThatsAdding);

            StringContent content =
                new StringContent(JsonSerializer.Serialize(twoFriends), Encoding.UTF8, "application/json");


            http.PatchAsync(request, content);
        }
    }
}