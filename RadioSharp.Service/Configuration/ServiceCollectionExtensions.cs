using Microsoft.Extensions.DependencyInjection;
using RadioSharp.Service.Data;
using RadioSharp.Service.Player;
using RadioSharp.Service.Stations;
using RadioSharp.Service.Search;

namespace RadioSharp.Service.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRadioServices(this IServiceCollection services)
        {
            services.AddSingleton<IRadioStationsService, RadioStationService>();
            services.AddSingleton<IDatabaseService, DatabaseService>();
            services.AddSingleton<IRadioPlayer, RadioPlayer>();
            services.AddSingleton<IRadioSearch, RadioSearch>();

            return services;
        }
    }
}
