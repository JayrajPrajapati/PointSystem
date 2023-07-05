using System.Data.SqlClient;

namespace PointSystem.Helper
{
    /// <summary>
    /// SqlHelper
    /// </summary>
    public class SqlHelper
    {
        //this field gets initialized at Startup.cs
        /// <summary>
        /// The connection string
        /// </summary>
        public static string ConnectionString;
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}