using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;

namespace Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // 1. Registrace LocalStorage
            builder.Services.AddBlazoredLocalStorage();

            // 2. Registrace tvého AuthService
            builder.Services.AddScoped<AuthService>();

            // 3. Registrace JwtHandleru (musí být Transient)
            builder.Services.AddTransient<JwtHandler>();

            // 4. Konfigurace HttpClient pomocí Factory
            // Tato část propojí tvůj JwtHandler s každým požadavkem, který HttpClient vyrobí
            builder.Services.AddHttpClient("ServerApi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7169/api/");
            })
            .AddHttpMessageHandler<JwtHandler>();

            // 5. Registrace výchozího HttpClient, který budou používat tvé Razor komponenty
            // @inject HttpClient Http nyní dostane instanci se zapojeným JwtHandlerem
            builder.Services.AddScoped(sp =>
                sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerApi"));

            await builder.Build().RunAsync();
        }
    }
}