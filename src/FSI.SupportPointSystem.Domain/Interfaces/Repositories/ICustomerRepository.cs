using System;
using System.Threading.Tasks;
using FSI.SupportPoint.Domain.Entities;
namespace FSI.SupportPoint.Domain.Interfaces.Repository
{
    public interface ICustomerRepository
    {
        Task UpsertAsync(Customer customer);
        Task<Customer?> GetByIdAsync(Guid id);
        Task<IEnumerable<Customer>> GetAllAsync();
    }
}