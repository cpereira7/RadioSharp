using RadioSharp.Service.Models;

namespace RadioSharp.Service.Stations
{
    public interface IRadioStationsService
    {
        IList<RadioStation> GetRadios();
        void SaveRadios(IList<RadioStation> radios);

        IList<RadioStation> GetLastPlayedRadios();
        void SaveLastPlayedRadio(RadioStation radio);

        void ReloadStations();
    }
}
