using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Models;
using Microsoft.JSInterop;
using Tier2.Model;

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
               
            }
            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                
                throw new Exception("Incorrect user id");
                
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
            StringContent content = new StringContent(JsonSerializer.Serialize(chatroom),Encoding.UTF8,"application/json");
            
            http.PostAsync("add", content);
        }

        public Task JoinChatRoom(string chatroomId)
        {
            throw new NotImplementedException();
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
    }
}