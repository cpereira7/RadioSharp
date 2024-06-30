using Newtonsoft.Json;
using RadioSharp.App.Models;

namespace RadioSharp.App.Parser
{
    public class JsonParsingService : IJsonParsingService
    {
        public string ConvertRadioStation(RadioStation radioStation)
        {
            return JsonConvert.SerializeObject(radioStation, Formatting.Indented);
        }

        public string ConvertRadioStations(IList<RadioStation> radios)
        {
            return JsonConvert.SerializeObject(radios, Formatting.Indented);
        }

        public IList<RadioStation> DeserializeRadioStations(string radioStations)
        {
            if (!string.IsNullOrEmpty(radioStations))
                return JsonConvert.DeserializeObject<IList<RadioStation>>(radioStations)!;

            return new List<RadioStation>();
        }
    }
}
