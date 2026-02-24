using Dapper;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Infrastructure.Context;
using FSI.SupportPointSystem.Infrastructure.Mappings;
using System.Data;

namespace FSI.SupportPointSystem.Infrastructure.Repositories
{
    public class SalesTeamRepository : ISalesTeamRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public SalesTeamRepository(DbConnectionFactory dbConnectionFactory)
            => _dbConnectionFactory = dbConnectionFactory;

        public async Task CreateAsync(SalesTeam salesTeam)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var parameters = SalesTeamMapper.ToParameters(salesTeam);

            await connection.ExecuteAsync(
                "SpCreateSalesTeam",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(SalesTeam salesTeam)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var parameters = SalesTeamMapper.ToParameters(salesTeam);

            await connection.ExecuteAsync(
                "SpUpdateSalesTeam",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<SalesTeam?> GetByIdAsync(Guid id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SpGetSalesTeamById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);

            return row != null ? SalesTeamMapper.ToDomain(row) : null;
        }

        public async Task<IEnumerable<SalesTeam>> GetAllAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var rows = await connection.QueryAsync<dynamic>("SpGetAllSalesTeams", commandType: CommandType.StoredProcedure);
            return rows.Select(row => (SalesTeam)SalesTeamMapper.ToDomain(row)).ToList();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "SpDeleteSalesTeam",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<SalesTeam?> GetByNameAsync(string name)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SELECT * FROM SalesTeam WHERE Name = @Name",
                new { Name = name },
                commandType: CommandType.Text); 
            return row != null ? SalesTeamMapper.ToDomain(row) : null;
        }

        public Task<SalesTeam?> GetByIdWithSellersAsync(Guid id)
        {
            throw new NotImplementedException("Implementação de carga relacional pendente.");
        }
    }
}