using RadioSharp.App.Models;

namespace RadioSharp.App.Data
{
    public interface IDatabaseService
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