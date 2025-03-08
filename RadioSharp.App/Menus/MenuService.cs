using RadioSharp.App.External;
using RadioSharp.App.Player;
using RadioSharp.Service.Models;
using RadioSharp.Service.Stations;

namespace RadioSharp.App.Menus
{
    public class MenuService : IMenuService
    {
        private IList<RadioStation> radios = new List<RadioStation>();
        private const int PageSize = 10;

        private readonly IRadioStationsService _radioStationsHandler;
        private readonly IRadioSearchHandler _radioSearch;
        private readonly IRadioPlayerHandler _radioPlayer;

        public MenuService(IRadioStationsService radioStationsHandler, IRadioSearchHandler radioSearch, IRadioPlayerHandler radioPlayer)
        {
            _radioStationsHandler = radioStationsHandler;
            _radioSearch = radioSearch;
            _radioPlayer = radioPlayer;
        }

        private static void DrawAppLogo()
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
                DisplayRadioMenu(currentPage, lastPlayed);

                Console.WriteLine($" Page {currentPage} of {totalPages}");
                Console.Write("\n\n [1..9] Radio, [N] Next, [P] Previous, [S] Search, [L] Last Played, [R] Reload, [Q] Quit/Back: ");

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
                    var menuActions = new Dictionary<string, Func<Task>>
                    {
                        { "N", () => { currentPage = Math.Min(currentPage + 1, totalPages); return Task.CompletedTask; } },
                        { "P", () => { currentPage = Math.Max(currentPage - 1, 1); return Task.CompletedTask; } },
                        { "S", async () => await DisplaySearchMenuAsync() },
                        { "L", async () => await DisplayPlayBackMenuAsync(true) },
                        { "R", () => { ReloadStations(); return Task.CompletedTask; } },
                        { "Q", () => { exit = true; return Task.CompletedTask; } }
                    };

                    if (menuActions.TryGetValue(key, out var action))
                    {
                        await action();
                    }
                }
            }
        }

        private void DisplayRadioMenu(int page, bool lastPlayed)
        {
            int startIndex = (page - 1) * PageSize;
            int endIndex = Math.Min(startIndex + PageSize, radios.Count);

            Console.WriteLine(lastPlayed ? "Last Played Radios:\n" : "Radio List:\n");
            Console.WriteLine($" Page {page}\n");

            for (int i = startIndex; i < endIndex; i++)
            {
                Console.WriteLine($"  {i + 1}. {radios[i].Name}");
            }

            Console.WriteLine();
        }

        private async Task DisplaySearchMenuAsync()
        {
            DrawAppLogo();

            await _radioSearch.SearchRadios();
        }

        private void GetStations(bool lastPlayed)
        {
            radios = lastPlayed ? _radioStationsHandler.GetLastPlayedRadios() : _radioStationsHandler.GetRadios();
        }

        private void ReloadStations()
        {
            _radioStationsHandler.ReloadStations();
        }
    }
}
