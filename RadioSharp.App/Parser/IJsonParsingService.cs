using RadioSharp.App.Models;

namespace RadioSharp.App.Parser
{
    public interface IJsonParsingService
    {
        string ConvertRadioStation(RadioStation radioStation);
        string ConvertRadioStations(IList<RadioStation> radios);
        IList<RadioStation> DeserializeRadioStations(string radioStations);
    }
}
