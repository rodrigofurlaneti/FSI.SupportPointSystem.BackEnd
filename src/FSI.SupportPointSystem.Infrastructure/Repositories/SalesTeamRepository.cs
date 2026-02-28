using Dapper;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Infrastructure.Context;
using FSI.SupportPointSystem.Infrastructure.Mappings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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
            // Ajuste: p_Id para bater com a Procedure MySQL e .ToString() para CHAR(36)
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SpGetSalesTeamById",
                new { p_Id = id.ToString() },
                commandType: CommandType.StoredProcedure);

            return row != null ? SalesTeamMapper.ToDomain(row) : null;
        }

        public async Task<IEnumerable<SalesTeam>> GetAllAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var rows = await connection.QueryAsync<dynamic>(
                "SpGetAllSalesTeams",
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => SalesTeamMapper.ToDomain(row))
                       .Where(t => t != null)
                       .Cast<SalesTeam>()
                       .ToList();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            // Ajuste: p_Id consistente com o padrão das procedures convertidas
            await connection.ExecuteAsync(
                "SpDeleteSalesTeam",
                new { p_Id = id.ToString() },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<SalesTeam?> GetByNameAsync(string name)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            // No MySQL com MySqlConnector, o uso de @Name em queries texto funciona bem
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