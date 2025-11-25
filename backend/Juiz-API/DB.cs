using MySql.Data;
using dotenv.net;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Data;

namespace DataBase
{
  public class DB
  {
    private string stringConnection;
    public DB(string name, int mat, int pont, string apro)
    {
      DotEnv.Load();
      stringConnection = Environment.GetEnvironmentVariable("StringConnection");
      Connection(name, mat, pont, apro);
    }
    private async Task Connection(string name, int mat, int pont, string apro)
    {
      MySqlConnection connection = new MySqlConnection(stringConnection);
      MySqlCommand command = new MySqlCommand("INSERT INTO `dados`(`name`, `mat`, `pont`, `aprov`) VALUES (@name, @mat, @pont, @apro)", connection);
      try
      {
        await connection.OpenAsync();
        if (connection.State == ConnectionState.Open)
        {
          command.Parameters.AddWithValue("@name", name);
          command.Parameters.AddWithValue("@mat", mat);
          command.Parameters.AddWithValue("@pont", pont);
          command.Parameters.AddWithValue("@apro", apro);
          await command.ExecuteNonQueryAsync();
        }
        else
        {
          Thread.Sleep(100);
          await Connection(name, mat, pont, apro);
        }
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