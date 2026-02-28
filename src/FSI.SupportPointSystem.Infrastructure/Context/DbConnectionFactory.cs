using System.Data;
using MySqlConnector; 
using Microsoft.Extensions.Configuration;

namespace FSI.SupportPointSystem.Infrastructure.Context
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("ConnectionString não encontrada.");
        }

        public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
    }
}