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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        public CustomerRepository(DbConnectionFactory dbConnectionFactory)
            => _dbConnectionFactory = dbConnectionFactory;
        public async Task UpsertAsync(Customer customer)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Id", customer.Id, DbType.Guid);
            parameters.Add("@CompanyName", customer.CompanyName, DbType.String);
            parameters.Add("@Cnpj", customer.Cnpj.Value, DbType.String);
            parameters.Add("@LatTarget", customer.LocationTarget.Latitude, DbType.Decimal);
            parameters.Add("@LogTarget", customer.LocationTarget.Longitude, DbType.Decimal);
            await connection.ExecuteAsync(
                "SpUpsertCustomer",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        public async Task UpdateAsync(Customer customer)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Id", customer.Id, DbType.Guid);
            parameters.Add("@CompanyName", customer.CompanyName, DbType.String);
            parameters.Add("@LatTarget", customer.LocationTarget.Latitude, DbType.Decimal);
            parameters.Add("@LogTarget", customer.LocationTarget.Longitude, DbType.Decimal);
            await connection.ExecuteAsync(
                "SpUpdateCustomer",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        public async Task DeleteAsync(Guid id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "SpDeleteCustomer",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SpGetCustomerById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
            return row != null ? CustomerMapper.ToDomain(row) : null;
        }
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var rows = await connection.QueryAsync<dynamic>(
                "SpGetAllCustomers",
                commandType: CommandType.StoredProcedure);
            return rows.Select(row => (Customer)CustomerMapper.ToDomain(row))
                       .Where(c => c != null)
                       .ToList();
        }
        public async Task<Customer?> GetByCnpjAsync(string cnpj)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SpGetCustomerByCnpj",
                new { Cnpj = cnpj },
                commandType: CommandType.StoredProcedure);
            return row != null ? CustomerMapper.ToDomain(row) : null;
        }
    }
}