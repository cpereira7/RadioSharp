using RadioBrowser.Models;
using RadioBrowser;
using RadioSharp.App.Models;
using RadioSharp.App.Parser;

namespace RadioSharp.App.Data
{
    internal class RadioSearch
    {
        protected RadioSearch() { }
        internal static async Task SearchRadios(string name = "", string countryCode = "", string language = "")
        {
            Console.WriteLine("\nSearching...");

            var options = new AdvancedSearchOptions() { Limit = 150 };

            if (!string.IsNullOrEmpty(name))
                options.Name = name;

            if (!string.IsNullOrEmpty(countryCode))
                options.Countrycode = countryCode;

            if (!string.IsNullOrEmpty(language))
                options.Language = language;

            var radioBrowser = new RadioBrowserClient();

            var advancedSearch = await radioBrowser.Search.AdvancedAsync(options);

            var resultList = new List<RadioStation>();

            if (advancedSearch != null && advancedSearch.Any())
            {
                Console.WriteLine($"Found {advancedSearch.Count} radio stations matching the criteria.");

                foreach (var result in advancedSearch)
                {
                    var radio = new RadioStation(result.Name.Trim(), result.Url.ToString());

                    if (!resultList.Contains(radio))
                    {
                        resultList.Add(radio);
                    }
                }

                RadioStationsExporter.ExportRadios(resultList);
            }
            else
            {
                Console.WriteLine("No radio stations found matching the criteria.");
            }

            Thread.Sleep(3000);

        }
    }
}
