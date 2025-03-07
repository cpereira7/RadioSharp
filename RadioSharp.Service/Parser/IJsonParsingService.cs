using RadioSharp.Service.Models;

namespace RadioSharp.Service.Parser
{
    public interface IJsonParsingService
    {
        string ConvertRadioStation(RadioStation radioStation);
        string ConvertRadioStations(IList<RadioStation> radios);
        IList<RadioStation> DeserializeRadioStations(string radioStations);
    }
}
