using RadioSharp.App.Models;

namespace RadioSharp.App.Stations
{
    public interface IRadioStationsHandler
    {
        IList<RadioStation> GetRadios();
        void SaveRadios(IList<RadioStation> radios);

        IList<RadioStation> GetLastPlayedRadios();
        void SaveLastPlayedRadio(RadioStation radio);

        void ReloadStations();
    }
}