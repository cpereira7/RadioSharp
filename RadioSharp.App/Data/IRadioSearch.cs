
namespace RadioSharp.App.Data
{
    public interface IRadioSearch
    {
        Task SearchRadios(string name = "", string countryCode = "", string language = "");
    }
}