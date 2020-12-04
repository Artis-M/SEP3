using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.SCMediator;
using Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tier2.Model;

namespace Application
{
    public class Program
    {
        public static void Main(string[] args)
        {ChatServiceImp chatServiceImp = new ChatServiceImp();
            Message message = null;
            chatServiceImp.sendMessage(message, "hi");
            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}