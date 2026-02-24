using Dapper;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Infrastructure.Context;
using FSI.SupportPointSystem.Infrastructure.Mappings;
using System.Data;

namespace FSI.SupportPointSystem.Infrastructure.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        public SellerRepository(DbConnectionFactory dbConnectionFactory)
            => _dbConnectionFactory = dbConnectionFactory;

        public async Task CreateAsync(Seller seller, string passwordHash)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var parameters = SellerMapper.ToParameters(seller, passwordHash);
            await connection.ExecuteAsync(
                "SpCreateSeller",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(Seller seller)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var parameters = SellerMapper.ToParameters(seller);
            await connection.ExecuteAsync(
                "SpUpdateSeller",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Seller?> GetByIdAsync(Guid id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SpGetSellerById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
            return row != null ? SellerMapper.ToDomain(row) : null;
        }

        public async Task<IEnumerable<Seller>> GetAllAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var rows = await connection.QueryAsync<dynamic>(
                "SpGetAllSellers",
                commandType: CommandType.StoredProcedure);
            return rows.Select(row => (Seller)SellerMapper.ToDomain(row)).ToList();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "SpDeleteSeller",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Seller>> GetBySalesTeamIdAsync(Guid salesTeamId)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var rows = await connection.QueryAsync<dynamic>(
                "SELECT * FROM Sellers WHERE SalesTeamId = @SalesTeamId",
                new { SalesTeamId = salesTeamId });
            return rows.Select(row => (Seller)SellerMapper.ToDomain(row)).ToList();
        }

        public async Task<Seller?> GetByCpfAsync(string cpf)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SpGetSellerByCpf",
                new { Cpf = cpf },
                commandType: CommandType.StoredProcedure);
            return row != null ? SellerMapper.ToDomain(row) : null;
        }
    }
}