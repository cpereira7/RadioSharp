using RadioSharp.Service.Models;

namespace RadioSharp.Service.RadioSearch
{
    public interface IRadioSearch
    {
        Task<IList<RadioStation>> SearchRadios(string name, string countryCode, string language);
    }
}
