using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Models;
using Microsoft.JSInterop;

namespace Services
{
    public class ChatroomServiceImp : IChatroomService
    {
        private string uri = "https://localhost:5004/chatrooms/";
        private readonly IJSRuntime jsRuntime;
        public Chatroom currentlySelectedChatroom;


        public async Task<List<Chatroom>> GetUsersChatrooms(string userId)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            HttpResponseMessage responseMessage = await http.GetAsync($"user/chatrooms/{userId}");
            List<Chatroom> Chatrooms = new List<Chatroom>();
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                string json = await responseMessage.Content.ReadAsStringAsync();
                Chatrooms = JsonSerializer.Deserialize<List<Chatroom>>(json);
            }

            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine(responseMessage.ToString());
            }

            return Chatrooms;
        }

        public async Task CreateChatRoom(Chatroom chatroom)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            Console.WriteLine(JsonSerializer.Serialize(chatroom));
            StringContent content =
                new StringContent(JsonSerializer.Serialize(chatroom), Encoding.UTF8, "application/json");

            await http.PostAsync("add", content);
        }

        public async Task JoinChatRoom(string chatroomId, string userID)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            string request = $"addUser/{chatroomId}";
            StringContent content =
                new StringContent(JsonSerializer.Serialize(userID), Encoding.UTF8, "application/json");
            await http.PatchAsync(request, content);
        }

        public async Task SetCurrentChatroom(string chatroomId)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            HttpResponseMessage responseMessage = await http.GetAsync($"{chatroomId}");
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                Chatroom chatroom =
                    JsonSerializer.Deserialize<Chatroom>(await responseMessage.Content.ReadAsStringAsync());
                currentlySelectedChatroom = chatroom;
            }
        }

        public Chatroom GetCurrentChatroom()
        {
            return currentlySelectedChatroom;
        }

        public async Task RemoveCurrentChatroom()
        {
            currentlySelectedChatroom = null;
        }

        public async Task LeaveChatRoom(string userID, string chatroomID)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            string request = $"removeUser/{chatroomID}";
            StringContent content =
                new StringContent(JsonSerializer.Serialize(userID), Encoding.UTF8, "application/json");
            await http.PatchAsync(request, content);
        }

        public async Task KickFromChatroom(string targetUserID, string chatroomID)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            string request = $"removeUser/{chatroomID}";

            StringContent content =
                new StringContent(JsonSerializer.Serialize(targetUserID), Encoding.UTF8, "application/json");

            await http.PatchAsync(request, content);
        }

        public async Task<List<Chatroom>> GetChatroomByTopic(string topic)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            String responseMessage = await http.GetStringAsync($"chatrooms/topic/{topic}");
            List<Chatroom> Chatrooms = new List<Chatroom>();
            Chatrooms = JsonSerializer.Deserialize<List<Chatroom>>(responseMessage);
            return Chatrooms;
        }

        public async Task EnterPrivateChatroom(string user, string user1)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            HttpResponseMessage responseMessage = await http.GetAsync($"private/{user}/{user1}");
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                Chatroom chatroom =
                    JsonSerializer.Deserialize<Chatroom>(await responseMessage.Content.ReadAsStringAsync());
                await SetCurrentChatroom(chatroom._id);
            }
        }

        public async Task DeleteChatRoom(string ChatroomID)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            await http.DeleteAsync($"{ChatroomID}");
        }
    }
}