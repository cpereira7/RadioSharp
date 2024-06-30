using RadioSharp.App.External;
using RadioSharp.App.Models;
using RadioSharp.App.Player;
using RadioSharp.App.Stations;

namespace RadioSharp.App.Menus
{
    public class MenuService : IMenuService
    {
        private IList<RadioStation> radios = new List<RadioStation>();
        private const int PageSize = 10;

        private readonly IRadioStationsHandler _radioStationsHandler;
        private readonly IRadioSearch _radioSearch;
        private readonly IRadioPlayer _radioPlayer;

        public MenuService(IRadioStationsHandler radioStationsHandler, IRadioSearch radioSearch, IRadioPlayer radioPlayer)
        {
            _radioStationsHandler = radioStationsHandler;
            _radioSearch = radioSearch;
            _radioPlayer = radioPlayer;
        }

        public void DrawAppLogo()
        {
            Console.Clear();
            Console.Title = $" ■ RadioSharp";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"   _____           _ _       _____ _                      ");
            Console.WriteLine(@"  |  __ \         | (_)     / ____| |                     ");
            Console.WriteLine(@"  | |__) |__ _  __| |_  ___| (___ | |__   __ _ _ __ _ __  ");
            Console.WriteLine(@"  |  _  // _` |/ _` | |/ _ \\___ \| '_ \ / _` | '__| '_ \ ");
            Console.WriteLine(@"  | | \ \ (_| | (_| | | (_) |___) | | | | (_| | |  | |_) |");
            Console.WriteLine(@"  |_|  \_\__,_|\__,_|_|\___/_____/|_| |_|\__,_|_|  | .__/ ");
            Console.WriteLine(@"                                                   | |    ");
            Console.WriteLine(@"                                                   |_|    ");
            Console.WriteLine();
        }

        public async Task DisplayPlayBackMenuAsync(bool lastPlayed = false)
        {
            int currentPage = 1;

            bool exit = false;
            while (!exit)
            {
                GetStations(lastPlayed);

                int totalPages = (int)Math.Ceiling((double)radios.Count / PageSize);

                if (currentPage > totalPages)
                    currentPage = 1;

                DrawAppLogo();
                DisplayRadioMenu(currentPage);

                Console.WriteLine($" Page {currentPage} of {totalPages}");
                Console.Write("\n\nSelect a radio by entering its number, N for Next, P for Previous, S to search, L for Last Played, Q to Quit: ");

                var input = Console.ReadLine();
                if (int.TryParse(input, out int selection))
                {
                    if (selection >= 1 && selection <= radios.Count)
                    {
                        DrawAppLogo();

                        var selectedRadio = radios[selection - 1];

                        _radioPlayer.PlayStream(selectedRadio, selection);
                    }
                }
                else
                {
                    var key = input!.Trim().ToUpper();
                    switch (key)
                    {
                        case "N":
                            currentPage = Math.Min(currentPage + 1, totalPages);
                            break;
                        case "P":
                            currentPage = Math.Max(currentPage - 1, 1);
                            break;
                        case "S":
                            await DisplaySearchMenuAsync();
                            break;
                        case "L":
                            await DisplayPlayBackMenuAsync(true);
                            break;
                        case "R":
                            ReloadStations();
                            break;
                        case "Q":
                            exit = true;
                            break;
                    }
                }
            }
        }

        public void DisplayRadioMenu(int page)
        {
            int startIndex = (page - 1) * PageSize;
            int endIndex = Math.Min(startIndex + PageSize, radios.Count);

            Console.WriteLine($" Page {page}\n");

            for (int i = startIndex; i < endIndex; i++)
            {
                Console.WriteLine($"  {i + 1}. {radios[i].Name}");
            }

            Console.WriteLine();
        }

        public async Task DisplaySearchMenuAsync()
        {
            DrawAppLogo();

            Console.WriteLine("Radio Stations Search (press enter to skip)");

            Console.Write("\n Radio Stations Name: ");
            var name = Console.ReadLine();
            Console.Write("\n Country Code: ");
            var country = Console.ReadLine();
            Console.Write("\n Language: ");
            var language = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(country) && string.IsNullOrWhiteSpace(language))
            {
                ConsoleHelpers.DisplayMessageWithDelay("No search parameters provided. Skipping search.", 3000);
                return;
            }

            await _radioSearch.SearchRadios(name, country, language);
        }

        private void GetStations(bool lastPlayed)
        {
            radios = _radioStationsHandler.GetRadios();

            if (lastPlayed)
                radios = _radioStationsHandler.GetLastPlayedRadios();
        }

        private void ReloadStations()
        {
            _radioStationsHandler.ReloadStations();
        }
    }
}
