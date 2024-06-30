using Newtonsoft.Json;
using RadioSharp.App.Models;

namespace RadioSharp.App.Parser
{
    public class RadioStationsHandler : IRadioStationsHandler
    {
        private readonly static string _directoryPath = @"data";
        private readonly static string _fileName = "radios.json";
        private readonly string _filePath;

        private readonly IJsonParsingService _jsonParsingService;

        public RadioStationsHandler(IJsonParsingService jsonParsingService)
        {
            _jsonParsingService = jsonParsingService;

            _filePath = Path.Combine(_directoryPath, _fileName);
        }

        public IList<RadioStation> GetRadios()
        {
            var radios = new List<RadioStation>();

            try
            {
                string jsonContent = File.ReadAllText(_filePath);
                radios = _jsonParsingService.DeserializeRadioStations(jsonContent).ToList();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: File 'radios.json' not found. {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: JSON deserialization failed for file 'radios.json'. {ex.Message}");
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
                if (File.Exists(_filePath))
                {
                    BackupExistingFile(_filePath);
                }

                WriteJsonToFile(_filePath, radios);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Error: Unauthorized access while writing to file 'radios.json'. {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: IO exception occurred while writing to file 'radios.json'. {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: An unexpected error occurred while exporting radio stations. {ex.Message}");
            }
        }

        private void WriteJsonToFile(string filePath, IList<RadioStation> radios)
        {
            string jsonContent = _jsonParsingService.ConvertRadioStations(radios);
            File.WriteAllText(filePath, jsonContent);
        }

        private static void BackupExistingFile(string filePath)
        {
            var totalFiles = Directory.GetFiles(_directoryPath).Length;
            string backupFileName = $"radios_old_{totalFiles}.json";
            string backupFilePath = Path.Combine(_directoryPath, backupFileName);
            File.Copy(filePath, backupFilePath);
        }
    }
}
