using RadioSharp.Service.Models;

namespace RadioSharp.Service.Data
{
    internal interface IDatabaseService
    {
        void InitDatabase();
        void SaveRadios(IList<RadioStation> radios);
        IList<RadioStation> GetRadios();
        void ClearRadios();
        int GetRadioStationCount();

        void AddPlayedRadio(RadioStation radio);
        IList<RadioStation> GetPlayedRadios();
    }
}