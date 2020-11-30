﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tier2.Model;

namespace Tier2.Http
{
    public class HttpConnect : IHttpConnect
    {
        private string url ="http://localhost:8080";
        private HttpClient client;
        private string chatroomDestination = "/messages";

        public HttpConnect(){
            client = new HttpClient();
        }

        public async Task SaveMessage(Message message){
            Console.Out.WriteLine("Save message");
            string messageJson = JsonSerializer.Serialize(message);
            HttpContent content = new StringContent(messageJson, Encoding.UTF8,"application/json");
            HttpResponseMessage response = await client.PostAsync(url + chatroomDestination + "/Add", content);
        }

        public async Task<List<Message>> GetMessages(){
            string messageString = await client.GetStringAsync(url + chatroomDestination);
            List<Message> messages = JsonSerializer.Deserialize<List<Message>>(messageString);
            return messages;
        }
    }
}