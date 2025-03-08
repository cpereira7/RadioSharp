using RadioSharp.Service.Models;

namespace RadioSharp.Service.Search
{
    public interface IRadioSearch
    {
        Task<IList<RadioStation>> SearchRadios(string name, string countryCode, string language);
    }
}
