using ImageUploadApp.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageUploadApp.Client
{

    public class Program
    {

        public static async Task Main(string[] args)
        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            InMemoryLog mLog = new();
            builder.Services.AddSingleton(mLog);


            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Logging.AddProvider(new InMemoryLoggerProvider(mLog));

            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress
                 = new Uri("https://imageloadupload.azurewebsites.net")
            });


            await builder.Build().RunAsync();
        }


    }

}
