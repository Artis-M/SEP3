using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
            
            Console.WriteLine("Doing the call");
            HttpResponseMessage responseMessage = await http.GetAsync($"user/chatrooms/{userId}");
            //Console.Out.WriteLine(responseMessage.StatusCode);
            List<Chatroom> Chatrooms = new List<Chatroom>();
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                
                Chatrooms = JsonSerializer.Deserialize<List<Chatroom>>(await responseMessage.Content.ReadAsStringAsync());
                //Console.Out.WriteLine("Chatrooms");
                //Console.Out.WriteLine(Chatrooms.Count);
               
            }
            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                
                throw new Exception("Incorrect user id");
                
            }
            return Chatrooms;
        }

        /*public async Task<List<Chatroom>> GetAllChatrooms()
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
        }*/


        public async Task CreateChatRoom(Chatroom chatroom)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            
            Console.WriteLine(JsonSerializer.Serialize(chatroom));
            StringContent content = new StringContent(JsonSerializer.Serialize(chatroom),Encoding.UTF8,"application/json");
            
            http.PostAsync("add", content);
        }

        public async Task JoinChatRoom(string chatroomId,string userID)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            string request = $"addUser/{chatroomId}";
            Console.Out.WriteLine($"{chatroomId},{userID}");
            StringContent content = new StringContent(JsonSerializer.Serialize(userID), Encoding.UTF8,"application/json");
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
                Chatroom chatroom = JsonSerializer.Deserialize<Chatroom>(await responseMessage.Content.ReadAsStringAsync());
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
            StringContent content = new StringContent(JsonSerializer.Serialize(userID), Encoding.UTF8,"application/json");
            http.PatchAsync(request, content);
        }

        public async Task KickFromChatroom(string targetUserID, string chatroomID)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            string request = $"removeUser/{chatroomID}";
            
            StringContent content = new StringContent(JsonSerializer.Serialize(targetUserID), Encoding.UTF8,"application/json");
            
            http.PatchAsync(request, content);
        }

        public async Task<List<Chatroom>> getChatroomByTopic(string topic)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            String responseMessage = await http.GetStringAsync($"chatrooms/topic/{topic}");
            //Console.Out.WriteLine(responseMessage.StatusCode);
            List<Chatroom> Chatrooms = new List<Chatroom>();
            Chatrooms = JsonSerializer.Deserialize<List<Chatroom>>(responseMessage);
                Console.Out.WriteLine(Chatrooms.Count);
                return Chatrooms;
        }

        public Task EnterPrivateChatroom(string user, string user1)
        {
            throw new NotImplementedException();
        }
    }
}