using Dapper;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Domain.ValueObjects;
using FSI.SupportPointSystem.Infrastructure.Context;
using System.Data;

namespace FSI.SupportPointSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public UserRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<User?> GetByCpfAsync(string cpf)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var result = await connection.QueryAsync<dynamic>(
                "SpGetUserByCpf",
                new { Cpf = cpf },
                commandType: CommandType.StoredProcedure
            );
            var row = result.FirstOrDefault();
            if (row == null) return null;
            Seller? seller = null;
            if (row.SellerId != null)
            {
                var sellerCpf = new Cpf(row.Cpf);
                seller = new Seller(
                    (Guid)row.SellerId,
                    (string)row.Name,
                    sellerCpf,
                    (string)(row.Email ?? string.Empty),
                    (string)(row.Phone ?? string.Empty),
                    (Guid?)row.SalesTeamId
                );
            }

            return new User(
                row.Id,
                row.Cpf,
                row.PasswordHash,
                row.Role,
                row.Name ?? string.Empty,
                seller
            );
        }

        public async Task AddAsync(User user)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            const string sql = @"
                INSERT INTO Users (Id, Cpf, PasswordHash, [Role]) 
                VALUES (@Id, @Cpf, @PasswordHash, @Role)";

            await connection.ExecuteAsync(sql, new
            {
                user.Id,
                Cpf = user.Cpf, // Assumindo que Cpf é um Value Object
                user.PasswordHash,
                user.Role
            });
        }
    }
}