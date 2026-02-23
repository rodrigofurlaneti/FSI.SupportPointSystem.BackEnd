using FSI.SupportPoint.Application.Dtos.Customer.Request;
using FSI.SupportPoint.Application.Dtos.Customer.Response;

namespace FSI.SupportPointSystem.Application.Interfaces
{
    public interface ICustomerAppService
    {
        Task RegisterCustomerAsync(CreateCustomerRequest request);
        Task<IEnumerable<CustomerResponse>> GetAllAsync();
        Task<CustomerResponse?> GetByIdAsync(Guid id);
        Task UpdateCustomerAsync(Guid id, UpdateCustomerRequest request);
        Task DeleteCustomerAsync(Guid id);
    }
}