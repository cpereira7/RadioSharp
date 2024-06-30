using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RadioSharp.App.Data;
using RadioSharp.App.External;
using RadioSharp.App.Menus;
using RadioSharp.App.Parser;
using RadioSharp.App.Player;
using RadioSharp.App.Stations;

namespace RadioSharp.App
{
    internal class Program
    {        
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var serviceProvider = host.Services;

            serviceProvider.GetRequiredService<IDatabaseService>().InitDatabase();

            await serviceProvider.GetRequiredService<IMenuService>().DisplayPlayBackMenuAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IJsonParsingService, JsonParsingService>();
                services.AddSingleton<IRadioStationsHandler, RadioStationsHandler>();
                services.AddSingleton<IRadioSearch, RadioSearch>();
                services.AddSingleton<IDatabaseService, DatabaseService>();
                services.AddSingleton<IMenuService, MenuService>();
                services.AddSingleton<IRadioPlayer, RadioPlayer>();
            });
    }
}
