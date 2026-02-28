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
            var parameters = CustomerMapper.ToParameters(customer);

            // As procedures no MySQL não requerem o prefixo @ na chamada do Dapper, 
            // mas os nomes dentro do DynamicParameters devem bater com os da Procedure.
            await connection.ExecuteAsync(
                "SpUpsertCustomer",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(Customer customer)
        {
            // Reutiliza a lógica de Upsert conforme sua implementação original
            await UpsertAsync(customer);
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            // Ajuste: O nome do parâmetro deve ser p_Id conforme definido na conversão SQL
            await connection.ExecuteAsync(
                "SpDeleteCustomer",
                new { p_Id = id.ToString() },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            // Ajuste: p_Id para consistência com o MySQL
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SpGetCustomerById",
                new { p_Id = id.ToString() },
                commandType: CommandType.StoredProcedure);

            return row != null ? CustomerMapper.ToDomain(row) : null;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var rows = await connection.QueryAsync<dynamic>(
                "SpGetAllCustomers",
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => CustomerMapper.ToDomain(row))
                       .Where(c => c != null)
                       .Cast<Customer>()
                       .ToList();
        }

        public async Task<Customer?> GetByCnpjAsync(string cnpj)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            // Ajuste: p_Cnpj para consistência
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SpGetCustomerByCnpj",
                new { p_Cnpj = cnpj },
                commandType: CommandType.StoredProcedure);

            return row != null ? CustomerMapper.ToDomain(row) : null;
        }
    }
}