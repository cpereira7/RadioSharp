using RadioBrowser.Models;
using RadioBrowser;
using RadioSharp.Service.Models;

namespace RadioSharp.Service.RadioSearch
{
    public class RadioSearch : IRadioSearch
    {
        public async Task<IList<RadioStation>> SearchRadios(string name, string countryCode, string language)
        {
            var options = new AdvancedSearchOptions() { Limit = 150 };

            if (!string.IsNullOrEmpty(name))
                options.Name = name;

            if (!string.IsNullOrEmpty(countryCode))
                options.Countrycode = countryCode;

            if (!string.IsNullOrEmpty(language))
                options.Language = language;

            var radioBrowser = new RadioBrowserClient();

            var advancedSearchResult = await radioBrowser.Search.AdvancedAsync(options);

            return ConvertRadioSearchResults(advancedSearchResult);
        }

        private static List<RadioStation> ConvertRadioSearchResults(List<StationInfo> searchResults)
        {
            var resultList = new List<RadioStation>();

            if (searchResults != null && searchResults.Any())
            {
                foreach (var result in searchResults)
                {
                    var radio = new RadioStation(result.Name.Trim(), result.Url.ToString());

                    if (!resultList.Contains(radio))
                    {
                        resultList.Add(radio);
                    }
                }
            }

            return resultList;
        }
    }
}
