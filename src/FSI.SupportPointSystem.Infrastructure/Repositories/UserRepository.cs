using Dapper;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Domain.ValueObjects;
using FSI.SupportPointSystem.Infrastructure.Context;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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

            // Ajuste: p_Cpf para bater com a Procedure MySQL convertida
            var result = await connection.QueryAsync<dynamic>(
                "SpGetUserByCpf",
                new { p_Cpf = cpf },
                commandType: CommandType.StoredProcedure
            );

            var row = result.FirstOrDefault();
            if (row == null) return null;

            Seller? seller = null;
            if (row.SellerId != null)
            {
                // Ajuste: Conversão de string (MySQL CHAR36) para Guid
                Guid sellerId = row.SellerId is Guid sGuid ? sGuid : Guid.Parse(row.SellerId.ToString());
                Guid? salesTeamId = null;

                if (row.SalesTeamId != null)
                    salesTeamId = row.SalesTeamId is Guid stGuid ? stGuid : Guid.Parse(row.SalesTeamId.ToString());

                var sellerCpf = new Cpf(row.Cpf);
                seller = new Seller(
                    sellerId,
                    (string)row.Name,
                    sellerCpf,
                    (string)(row.Email ?? string.Empty),
                    (string)(row.Phone ?? string.Empty),
                    salesTeamId
                );
            }

            // Ajuste: Conversão do Id do User para Guid
            Guid userId = row.Id is Guid uGuid ? uGuid : Guid.Parse(row.Id.ToString());

            return new User(
                userId,
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

            // Ajuste: Removido os colchetes [Role] (sintaxe SQL Server) 
            // No MySQL usa-se crase `Role` se for palavra reservada, ou apenas Role.
            const string sql = @"
                INSERT INTO Users (Id, Cpf, PasswordHash, Role) 
                VALUES (@Id, @Cpf, @PasswordHash, @Role)";

            await connection.ExecuteAsync(sql, new
            {
                Id = user.Id.ToString(), // Passando como string para CHAR(36)
                Cpf = user.Cpf,
                user.PasswordHash,
                user.Role
            });
        }
    }
}