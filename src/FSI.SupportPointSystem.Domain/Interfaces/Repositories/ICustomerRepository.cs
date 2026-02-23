using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FSI.SupportPointSystem.Domain.Entities;

namespace FSI.SupportPointSystem.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task UpsertAsync(Customer customer);
        Task<Customer?> GetByIdAsync(Guid id);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Guid id);
    }
}