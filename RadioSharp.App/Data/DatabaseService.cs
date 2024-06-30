using DuckDB.NET.Data;
using RadioSharp.App.Models;
using RadioSharp.App.Parser;
using System.Text;

namespace RadioSharp.App.Data
{
    public class DatabaseService : IDatabaseService
    {
        const string dataSource = "radios.db";
        private readonly DuckDBConnection duckDBConnection;
        private readonly IJsonParsingService _jsonParsingService;

        public DatabaseService(IJsonParsingService jsonParsingService)
        {
            duckDBConnection = new DuckDBConnection($"Data Source={dataSource}");
            
            _jsonParsingService = jsonParsingService;
        }

        public void InitDatabase()
        {
            duckDBConnection.Open();

            using var command = duckDBConnection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS played_radios(time DATETIME, radio JSON);";
            command.ExecuteNonQuery();

            command.CommandText = "CREATE TABLE IF NOT EXISTS radios(radios JSON);";
            command.ExecuteNonQuery();
        }

        public void SaveRadios(IList<RadioStation> radios)
        {
            if (radios == null)
                return;

            var radioValue = _jsonParsingService.ConvertRadioStations(radios);

            using var command = duckDBConnection.CreateCommand();
            command.CommandText = "INSERT INTO radios VALUES ($radios);";
            command.Parameters.Add(new DuckDBParameter("radios", radioValue));
            command.ExecuteNonQuery();
        }

        public IList<RadioStation> GetRadios()
        {
            using var command = duckDBConnection.CreateCommand();
            command.CommandText = "SELECT * FROM radios;";
            using var reader = command.ExecuteReader();

            return ReadQueryResults(reader);
        }

        public void ClearRadios()
        {
            using var command = duckDBConnection.CreateCommand();

            command.CommandText = "DELETE FROM radios;";
            command.ExecuteNonQuery();
        }

        public int GetRadioStationCount()
        {
            using var command = duckDBConnection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM radios";
            var count = command.ExecuteScalar();

            if (int.TryParse(count!.ToString(), out int result))
                return result;

            return 0;
        }

        public void AddPlayedRadio(RadioStation radio)
        {
            if (radio == null)
                return;

            var radioValue = _jsonParsingService.ConvertRadioStation(radio);

            using var command = duckDBConnection.CreateCommand();
            command.CommandText = "INSERT INTO played_radios VALUES ($time, $radio);";
            command.Parameters.Add(new DuckDBParameter("time", DateTime.UtcNow));
            command.Parameters.Add(new DuckDBParameter("radio", radioValue));
            command.ExecuteNonQuery();
        }

        public IList<RadioStation> GetPlayedRadios()
        {
            using var command = duckDBConnection.CreateCommand();
            command.CommandText =
                "WITH OrderedRadios AS (SELECT time, radio, " +
                "ROW_NUMBER() OVER(PARTITION BY radio ORDER BY time DESC) AS rn FROM played_radios ) " +
                "SELECT json_group_array(radio) AS radio_array " +
                "FROM(SELECT radio FROM OrderedRadios WHERE rn = 1 ORDER BY time DESC LIMIT 10 ); ";

            using var reader = command.ExecuteReader();
            
            return ReadQueryResults(reader);
        }

        private IList<RadioStation> ReadQueryResults(DuckDBDataReader reader)
        {
            var stringBuilder = new StringBuilder();
            while (reader.Read())
            {
                try
                {
                    stringBuilder.Append(reader.GetString(0));
                }
                catch
                {
                    return new List<RadioStation>();
                }
            }

            return _jsonParsingService.DeserializeRadioStations(stringBuilder.ToString())!;
        }
    }
}
