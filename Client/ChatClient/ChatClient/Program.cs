using System.Threading.Tasks;
using ChatClient;
using Models.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Services;

namespace Models
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddSingleton<IChatService, ChatService>();
            builder.Services.AddSingleton<IAccountService, AccountService>();
            builder.Services.AddSingleton<IChatroomService, ChatroomServiceImp>();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProvider>();
            builder.Services.AddAuthorizationCore();
            await builder.Build().RunAsync();
        }
    }
}