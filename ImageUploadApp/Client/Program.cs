using ImageUploadApp.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace ImageUploadApp.Client
{

    public class Program
    {

        public static async Task Main(string[] args)
        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            #region logger
                InMemoryLog mLog = new();
                builder.Services.AddSingleton(mLog);
                builder.Logging.SetMinimumLevel(LogLevel.Debug);
                builder.Logging.AddProvider(new InMemoryLoggerProvider(mLog));
            #endregion


            builder.RootComponents.Add<App>("#app");

            #region Progress Bar
                builder.Services.AddLoadingBar(options =>
                {
                    options.LoadingBarColor = "blue";
                });
                builder.UseLoadingBar();
            #endregion 

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress
                 = new Uri("https://imageloadupload.azurewebsites.net")
            }.EnableIntercept(sp));


            await builder.Build().RunAsync();
        }


    }

}
