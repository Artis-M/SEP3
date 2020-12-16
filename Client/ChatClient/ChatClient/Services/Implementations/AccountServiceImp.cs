using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Models;

namespace Services
{
    public class AccountService : IAccountService
    {
        private string uri = "https://localhost:5004/accounts/";
        private readonly IJSRuntime jsRuntime;

        public AccountService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

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
            string request = $"login";

            http.DefaultRequestHeaders.Add("username", username);
            http.DefaultRequestHeaders.Add("password", hashedPassword);

            HttpResponseMessage responseMessage = await http.GetAsync(request);
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

        public async Task<Account> GetUser(string username)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            Account account = null;
            try
            {
                string json = await http.GetStringAsync(uri + $"user/username/{username}");
                account = JsonSerializer.Deserialize<Account>(json);
                
            }
            catch (Exception e)

            {
                return null;
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
        }

        public async Task RemoveFriend(string userId, string friendId)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            string request = $"user/removeFriend/{userId}";
            StringContent content =
                new StringContent(JsonSerializer.Serialize(friendId), Encoding.UTF8, "application/json");
            await http.PatchAsync(request, content);
        }

        public async Task AddFriend(string UserID, Account userAccount)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            string request = $"user/{UserID}";

            string userJson = await http.GetStringAsync(request);
            User targetUser = JsonSerializer.Deserialize<User>(userJson);

            userAccount.friends.Add(targetUser);
            User theOneThatsAdding = userAccount;

            await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser",
                JsonSerializer.Serialize(userAccount));

            List<User> twoFriends = new List<User>();
            twoFriends.Add(targetUser);
            twoFriends.Add(theOneThatsAdding);
            StringContent content =
                new StringContent(JsonSerializer.Serialize(twoFriends), Encoding.UTF8, "application/json");
            request = $"addFriend";
            await http.PatchAsync(request, content);
        }

        public async Task DeleteProfile(string userID)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            string request = $"{userID}";
            await http.DeleteAsync(request);
        }

        public async Task AddTopicToProfile(string topic, string userId)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            string response = uri + $"topic/add/{userId}";
            try
            {
                string serialized = JsonSerializer.Serialize(topic);
                StringContent content = new StringContent(serialized, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await http.PostAsync(response, content);
                Console.Out.WriteLine(responseMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task RemoveTopicFromProfile(string topic, string userId)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            string response = uri + $"topic/remove/{userId}/{topic}";
            try
            {
                HttpResponseMessage responseMessage = await http.DeleteAsync(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task EditProfile(Account account)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            string response = uri + $"editAccount";
            SHA384CryptoServiceProvider sha = new SHA384CryptoServiceProvider();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(account.Pass);
            byte[] hashedBytes = sha.ComputeHash(passwordBytes);
            account.Pass = Convert.ToBase64String(hashedBytes);
            string serialized = JsonSerializer.Serialize(account);
            StringContent content = new StringContent(serialized, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await http.PatchAsync(response, content);
        }
    }
}