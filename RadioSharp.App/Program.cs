using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RadioSharp.App.External;
using RadioSharp.App.Menus;
using RadioSharp.App.Player;
using RadioSharp.Service.Configuration;

namespace RadioSharp.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var serviceProvider = host.Services;

            await serviceProvider.GetRequiredService<IMenuService>().DisplayPlayBackMenuAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddRadioServices();

                services.AddSingleton<IRadioSearchHandler, RadioSearchHandler>();
                services.AddSingleton<IMenuService, MenuService>();
                services.AddSingleton<IRadioPlayerHandler, RadioPlayerHandler>();
            });
    }
}
