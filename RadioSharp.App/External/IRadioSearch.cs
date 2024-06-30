namespace RadioSharp.App.External
{
    public interface IRadioSearch
    {
        Task SearchRadios(string name = "", string countryCode = "", string language = "");
    }
}