using Newtonsoft.Json;
using RadioSharp.App.Models;

namespace RadioSharp.App.Parser
{
    internal class RadioStationsExporter
    {
        protected RadioStationsExporter() { }

        public static void ExportRadios(IList<RadioStation> radios)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(radios, Formatting.Indented);
                File.WriteAllText(@"data/radios.json", jsonContent);
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
    }

}
