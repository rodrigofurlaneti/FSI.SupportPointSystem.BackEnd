using Dapper;
using System.Data;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repository;
using FSI.SupportPointSystem.Infrastructure.Context;

namespace FSI.SupportPointSystem.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public CustomerRepository(DbConnectionFactory dbConnectionFactory) => _dbConnectionFactory = dbConnectionFactory;

        public async Task UpsertAsync(Customer customer)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("p_CompanyName", customer.CompanyName, DbType.String);
            parameters.Add("p_Cnpj", customer.Cnpj.Value, DbType.String);
            parameters.Add("p_LatTarget", customer.LocationTarget.Latitude, DbType.Decimal);
            parameters.Add("p_LogTarget", customer.LocationTarget.Longitude, DbType.Decimal);
            await connection.ExecuteAsync("CALL SpUpsertCustomer(@p_CompanyName, @p_Cnpj, @p_LatTarget, @p_LogTarget)", parameters);
        }

        public async Task<Customer?> GetByCnpjAsync(string cnpj)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Customers WHERE Cnpj = @Cnpj";

            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Cnpj = cnpj });

            if (row == null) return null;

            return new Customer(
                (string)row.companyname,
                new FSI.SupportPointSystem.Domain.ValueObjects.Cnpj((string)row.cnpj),
                new FSI.SupportPointSystem.Domain.ValueObjects.Coordinates((decimal)row.latitudetarget, (decimal)row.longitudetarget)
            );
        }
    }
}