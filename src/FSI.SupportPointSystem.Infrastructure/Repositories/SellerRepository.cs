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
    public class SellerRepository : ISellerRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        public SellerRepository(DbConnectionFactory dbConnectionFactory)
            => _dbConnectionFactory = dbConnectionFactory;

        public async Task CreateAsync(Seller seller, string passwordHash)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            // O SellerMapper.ToParameters já gera os parâmetros com o prefixo p_ e IDs como string
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
            // Ajuste: p_Id consistente com o MySQL e conversão para string do CHAR(36)
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SpGetSellerById",
                new { p_Id = id.ToString() },
                commandType: CommandType.StoredProcedure);

            return row != null ? SellerMapper.ToDomain(row) : null;
        }

        public async Task<IEnumerable<Seller>> GetAllAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var rows = await connection.QueryAsync<dynamic>(
                "SpGetAllSellers",
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => SellerMapper.ToDomain(row))
                       .Where(s => s != null)
                       .Cast<Seller>()
                       .ToList();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            // Ajuste: Soft Delete via procedure conforme sua lógica original
            await connection.ExecuteAsync(
                "SpDeleteSeller",
                new { p_Id = id.ToString() },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Seller>> GetBySalesTeamIdAsync(Guid salesTeamId)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            // Query direta usando @ para o driver MySqlConnector
            var rows = await connection.QueryAsync<dynamic>(
                "SELECT * FROM Sellers WHERE SalesTeamId = @SalesTeamId",
                new { SalesTeamId = salesTeamId.ToString() });

            return rows.Select(row => SellerMapper.ToDomain(row))
                       .Where(s => s != null)
                       .Cast<Seller>()
                       .ToList();
        }

        public async Task<Seller?> GetByCpfAsync(string cpf)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            // Ajuste: p_Cpf para bater com os parâmetros da procedure no MySQL
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SpGetSellerByCpf",
                new { p_Cpf = cpf },
                commandType: CommandType.StoredProcedure);

            return row != null ? SellerMapper.ToDomain(row) : null;
        }
    }
}