using RadioSharp.Service.Models;
using RadioSharp.App.Stations;
using RadioSharp.App.Menus;
using RadioSharp.Service.RadioSearch;

namespace RadioSharp.App.External
{
    public class RadioSearchHandler : IRadioSearchHandler
    {
        private readonly IRadioStationsHandler _radioStationsHandler;
        private readonly IRadioSearch _radioSearch;

        public RadioSearchHandler(IRadioStationsHandler radioStationsHandler, IRadioSearch radioSearch)
        {
            _radioStationsHandler = radioStationsHandler;
            _radioSearch = radioSearch;
        }

        public async Task SearchRadios()
        {
            Console.WriteLine("Radio Stations Search (press enter to skip)");

            Console.Write("\n Radio Stations Name (press enter to skip): ");
            var name = Console.ReadLine();
            Console.Write("\n Country Code (press enter to skip): ");
            var country = Console.ReadLine();
            Console.Write("\n Language (press enter to skip): ");
            var language = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(country) && string.IsNullOrWhiteSpace(language))
            {
                ConsoleHelpers.DisplayMessageWithDelay("No search parameters provided. Skipping search.");
                return;
            }

            Console.WriteLine("\nSearching...");

            var radioSearch = await _radioSearch.SearchRadios(name!, country!, language!);

            if (radioSearch != null && radioSearch.Any())
            {
                _radioStationsHandler.SaveRadios(radioSearch);

                ConsoleHelpers.DisplayMessageWithDelay($"Found {radioSearch.Count} radio stations matching the criteria.");
            }
            else
            {
                ConsoleHelpers.DisplayMessageWithDelay("No radio stations found matching the criteria.");
            }
        }
    }
}
