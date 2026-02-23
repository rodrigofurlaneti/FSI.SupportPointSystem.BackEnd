using System;
using System.Threading.Tasks;
using FSI.SupportPointSystem.Domain.Entities;
namespace FSI.SupportPointSystem.Domain.Interfaces.Repository
{
    public interface ICustomerRepository
    {
        Task UpsertAsync(Customer customer);
        Task<Customer?> GetByIdAsync(Guid id);
        Task<IEnumerable<Customer>> GetAllAsync();
    }
}