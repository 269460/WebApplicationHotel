using MySqlConnector;

namespace TestSqlConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the connection test...");
            var connectionString = "Server=localhost;Port=4406;Database=mydb;User Id=repl2;Password=111;SslMode=None;AllowPublicKeyRetrieval=True;";
            //var connectionString = "Server=localhost;Database=mydb;User Id=repl;Password=111;SslMode=None;AllowPublicKeyRetrieval=True;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    Console.WriteLine("Attempting to open the connection...");
                    connection.Open();
                    Console.WriteLine("Connection successful!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Connection failed: {ex.Message}");
                }
            }

            Console.WriteLine("Connection test finished.");
        }
    }
}
