using Dapper;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
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
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SpGetUserByCpf",
                new { Cpf = cpf },
                commandType: CommandType.StoredProcedure
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