using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;

namespace Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { 
                BaseAddress = new Uri("http://localhost:5036/api/") 
            });

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<AuthService>();

            await builder.Build().RunAsync();
        }
    }
}
