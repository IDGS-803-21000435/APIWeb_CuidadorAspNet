using Microsoft.Data.SqlClient;
using System.Data;

namespace Cuidador.Data
{
    public class DataContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("cadenaSQLRemota");
        }

        public IDbConnection createConnection() => new SqlConnection(_connectionString);

    }
}
