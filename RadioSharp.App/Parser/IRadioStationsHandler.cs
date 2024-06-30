using RadioSharp.App.Models;

namespace RadioSharp.App.Parser
{
    public interface IRadioStationsHandler
    {
        IList<RadioStation> GetRadios();
        void SaveRadios(IList<RadioStation> radios);
    }
}