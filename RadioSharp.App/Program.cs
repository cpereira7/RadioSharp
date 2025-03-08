using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RadioSharp.App.External;
using RadioSharp.App.Menus;
using RadioSharp.App.Parser;
using RadioSharp.App.Player;
using RadioSharp.App.Stations;
using RadioSharp.Service.Data;
using RadioSharp.Service.Parser;
using RadioSharp.Service.Player;
using RadioSharp.Service.RadioSearch;

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
                services.AddSingleton<IRadioSearchHandler, RadioSearchHandler>();
                services.AddSingleton<IDatabaseService, DatabaseService>();
                services.AddSingleton<IMenuService, MenuService>();
                services.AddSingleton<IRadioPlayerHandler, RadioPlayerHandler>();
                services.AddSingleton<IRadioPlayer, RadioPlayer>();
                services.AddSingleton<IRadioSearch, RadioSearch>();
            });
    }
}
