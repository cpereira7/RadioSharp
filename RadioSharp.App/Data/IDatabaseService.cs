using RadioSharp.App.Models;

namespace RadioSharp.App.Data
{
    public interface IDatabaseService
    {
        void AddRadio(RadioStation radio);
        IList<RadioStation> GetRadios();
        void InitDatabase();
    }
}