using Newtonsoft.Json;
using RadioSharp.App.Models;

namespace RadioSharp.App.Parser
{
    internal class RadioStationsExporter
    {
        private readonly static string _directoryPath = @"data";
        private readonly static string _fileName = "radios.json";
        
        protected RadioStationsExporter() { }

        public static void ExportRadios(IList<RadioStation> radios)
        {
            try
            {
                string filePath = Path.Combine(_directoryPath, _fileName);

                if (File.Exists(filePath))
                {
                    BackupExistingFile(filePath);
                }

                WriteJsonToFile(filePath, radios);
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

        private static void WriteJsonToFile(string filePath, IList<RadioStation> radios)
        {
            string jsonContent = JsonConvert.SerializeObject(radios, Formatting.Indented);
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
