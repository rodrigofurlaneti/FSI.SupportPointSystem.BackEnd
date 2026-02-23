using System.Data;
using Microsoft.Data.SqlClient; 
using Microsoft.Extensions.Configuration;

namespace FSI.SupportPointSystem.Infrastructure.Context
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("ConnectionString 'DefaultConnection' não encontrada no appsettings.json.");
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}