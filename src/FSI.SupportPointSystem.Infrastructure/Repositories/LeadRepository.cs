using System.Data;
using Dapper;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Infrastructure.Context;

namespace FSI.SupportPointSystem.Infrastructure.Repositories
{
    public class LeadRepository : ILeadRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public LeadRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Lead>> GetAllAvailableAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            return await connection.QueryAsync<Lead>(
                "sp_GetAllAvailableLeads",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Lead?> GetByIdAsync(Guid id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Lead>(
                "sp_GetLeadById",
                new { p_Id = id.ToString() },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateStatusAsync(Guid id, string status)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                "sp_UpdateLeadStatus",
                new
                {
                    p_Id = id.ToString(),
                    p_Status = status
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}