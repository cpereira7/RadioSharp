using Newtonsoft.Json;
using RadioSharp.Service.Models;

namespace RadioSharp.Service.Parser
{
    internal static class JsonParsingUtils
    {
        public static string ConvertRadioStation(RadioStation radioStation)
        {
            return JsonConvert.SerializeObject(radioStation, Formatting.Indented);
        }

        public static string ConvertRadioStations(IList<RadioStation> radios)
        {
            return JsonConvert.SerializeObject(radios, Formatting.Indented);
        }

        public static IList<RadioStation> DeserializeRadioStations(string radioStations)
        {
            if (!string.IsNullOrEmpty(radioStations))
                return JsonConvert.DeserializeObject<IList<RadioStation>>(radioStations)!;

            return [];
        }
    }
}
