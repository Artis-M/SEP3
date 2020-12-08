using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Models;
using Microsoft.JSInterop;
using Tier2.Model;

namespace Services
{
    public class ChatroomServiceImp : IChatroomService
    {
        
        private string uri = "https://localhost:5004/chatrooms/user/chatrooms";
        private readonly IJSRuntime jsRuntime;
        
        public async Task<List<Chatroom>> GetUsersChatrooms()
        {
            
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            
            string userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
            Account useraccount;
            if (!string.IsNullOrEmpty(userAsJson))
            {
                useraccount = JsonSerializer.Deserialize<Account>(userAsJson);
            }
            else
            {
                return null;
            }
            
            HttpResponseMessage responseMessage = await http.GetAsync(useraccount._id);
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
    }
}