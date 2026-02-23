using Dapper;
using System.Data;
using FSI.SupportPointSystem.Domain.Interfaces.Repository;
using FSI.SupportPointSystem.Infrastructure.Context;

namespace FSI.SupportPointSystem.Infrastructure.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public SellerRepository(DbConnectionFactory dbConnectionFactory) => _dbConnectionFactory = dbConnectionFactory;

        public async Task CreateAsync(string cpf, string passwordHash, string name, string phone)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("p_Cpf", cpf, DbType.String);
            parameters.Add("p_PasswordHash", passwordHash, DbType.String);
            parameters.Add("p_Name", name, DbType.String);
            parameters.Add("p_Phone", phone, DbType.String);
            await connection.ExecuteAsync("CALL SpCreateSeller(@p_Cpf, @p_PasswordHash, @p_Name, @p_Phone)", parameters);
        }

        public async Task<Seller?> GetByIdAsync(Guid id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            const string sql = @"
                SELECT s.Id, s.Name, s.Phone, u.Cpf 
                FROM Sellers s
                INNER JOIN Users u ON s.UserId = u.Id
                WHERE s.Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<Seller>(sql, new { Id = id });
        }
    }
}