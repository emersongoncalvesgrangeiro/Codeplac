using MySql.Data;
using dotenv.net;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace DataBase
{
  public class DB
  {
    private string stringConnection;
    public DB()
    {
      DotEnv.Load();
      stringConnection = Environment.GetEnvironmentVariable("ConnectionString");
    }
    private async Task Connection(string table_name, string name, string mat, int pont, string apro)
    {
      var connection = new MySql.Data.MySqlClient.MySqlConnection(stringConnection);
      try
      {
        await connection.OpenAsync();
        MySqlCommand command = new MySqlCommand();
        command.Connection = connection;
        string sql = $@"INSERT INTO ${table_name} (nome, matricula, pontuacao, aprovacao) VALUES (@name, @mat, @pont, @apro);";
        command.CommandText = sql;
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@mat", mat);
        command.Parameters.AddWithValue("@pont", pont);
        command.Parameters.AddWithValue("@apro", apro);
        var commandrunner = command.ExecuteNonQueryAsync();
      }
      catch (MySqlException e)
      {
        Console.WriteLine($"Error: ${e.Message}");
      }
      finally
      {
        connection.Close();
      }
    }
  }
}