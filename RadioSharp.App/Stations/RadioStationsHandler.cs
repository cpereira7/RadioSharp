using RadioSharp.Service.Data;
using RadioSharp.Service.Models;
using RadioSharp.Service.Parser;

namespace RadioSharp.App.Stations
{
    public class RadioStationsHandler : IRadioStationsHandler
    {
        // pre-loaded stations
        private readonly static string _directoryPath = @"data";
        private readonly static string _fileName = "radios.json";
        private readonly string _filePath;

        private readonly IJsonParsingService _jsonParsingService;
        private readonly IDatabaseService _databaseService;

        public RadioStationsHandler(IJsonParsingService jsonParsingService, IDatabaseService databaseService)
        {
            _jsonParsingService = jsonParsingService;
            _databaseService = databaseService;
            _filePath = Path.Combine(_directoryPath, _fileName);
        }

        public IList<RadioStation> GetRadios()
        {
            var radios = new List<RadioStation>();

            try
            {
                if (_databaseService.GetRadioStationCount() == 0)
                {
                    var tempStations = File.ReadAllText(_filePath);
                    radios = [.. _jsonParsingService.DeserializeRadioStations(tempStations)];
                }
                else
                {
                    radios = [.. _databaseService.GetRadios()];
                }

            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: IO exception occurred while reading the file 'radios.json'. {ex.Message}");
            }

            return radios;
        }

        public void SaveRadios(IList<RadioStation> radios)
        {
            try
            {
                _databaseService.ClearRadios();
                _databaseService.SaveRadios(radios);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: An unexpected error occurred while exporting radio stations. {ex.Message}");
            }
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
