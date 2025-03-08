using RadioSharp.Service.Data;
using RadioSharp.Service.Models;
using RadioSharp.Service.Parser;

namespace RadioSharp.Service.Stations
{
    internal class RadioStationService : IRadioStationsService
    {
        // pre-loaded stations
        private readonly static string _directoryPath = @"data";
        private readonly static string _fileName = "radios.json";
        private readonly string _filePath;

        private readonly IDatabaseService _databaseService;

        public RadioStationService()
        {
            _databaseService = new DatabaseService();
            _filePath = Path.Combine(_directoryPath, _fileName);

            _databaseService.InitDatabase();
        }

        public IList<RadioStation> GetRadios()
        {
            List<RadioStation> radios;

            if (_databaseService.GetRadioStationCount() == 0)
            {
                var tempStations = File.ReadAllText(_filePath);
                radios = [.. JsonParsingUtils.DeserializeRadioStations(tempStations)];
            }
            else
            {
                radios = [.. _databaseService.GetRadios()];
            }

            return radios;
        }

        public void SaveRadios(IList<RadioStation> radios)
        {
            _databaseService.ClearRadios();
            _databaseService.SaveRadios(radios);
        }

        public IList<RadioStation> GetLastPlayedRadios()
        {
            return _databaseService.GetPlayedRadios();
        }

        public void SaveLastPlayedRadio(RadioStation radio)
        {
            _databaseService.AddPlayedRadio(radio);
        }

        public void ReloadStations()
        {
            _databaseService.ClearRadios();
        }
    }
}
