using DuckDB.NET.Data;
using Newtonsoft.Json;
using RadioSharp.App.Models;
using System.Text;

namespace RadioSharp.App.Data
{
    internal class Database
    {
        const string dataSource = "radios.db";
        private readonly DuckDBConnection duckDBConnection;

        public Database()
        {
            duckDBConnection = new DuckDBConnection($"Data Source={dataSource}");
        }

        public void InitDatabase()
        {
            duckDBConnection.Open();

            using var command = duckDBConnection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS played_radios(time DATETIME, radio JSON);";
            command.ExecuteNonQuery();
        }

        public void AddRadio(RadioStation radio)
        {
            if (radio == null)
                return;

            var radioValue = ConvertRadioStation(radio);

            using var command = duckDBConnection.CreateCommand();
            command.CommandText = "INSERT INTO played_radios VALUES ($time, $radio);";
            command.Parameters.Add(new DuckDBParameter("time", DateTime.UtcNow));
            command.Parameters.Add(new DuckDBParameter("radio", radioValue));
            command.ExecuteNonQuery();
        }

        public IList<RadioStation> GetRadios()
        {
            using var command = duckDBConnection.CreateCommand();
            command.CommandText = "SELECT json_group_array(radio) AS radio_array " +
                "FROM (SELECT radio FROM played_radios ORDER BY time DESC LIMIT 10);";
            using var reader = command.ExecuteReader();

            var stringBuilder = new StringBuilder();
            while (reader.Read())
            {
                stringBuilder.Append(reader.GetString(0));
            }

            return DeserializeRadioStations(stringBuilder.ToString())!;
        }

        private static string ConvertRadioStation(RadioStation radioStation)
        {
            return JsonConvert.SerializeObject(radioStation, Formatting.Indented);
        }

        private static IList<RadioStation> DeserializeRadioStations(string radioStations)
        {
            if (!string.IsNullOrEmpty(radioStations))
                return JsonConvert.DeserializeObject<IList<RadioStation>>(radioStations);

            return new List<RadioStation>();
        }
    }
}
