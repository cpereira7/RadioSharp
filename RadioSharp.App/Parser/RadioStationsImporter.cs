using Newtonsoft.Json;
using RadioSharp.App.Models;

namespace RadioSharp.App.Parser
{
    internal class RadioStationsImporter
    {
        protected RadioStationsImporter() { }

        public static IList<RadioStation> GetRadios()
        {
            var radios = new List<RadioStation>();

            try
            {
                string jsonContent = File.ReadAllText(@"data/radios.json");
                radios = JsonConvert.DeserializeObject<List<RadioStation>>(jsonContent);
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
    }
}
