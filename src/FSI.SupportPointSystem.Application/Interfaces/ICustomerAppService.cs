using FSI.SupportPointSystem.Application.Dtos;

namespace FSI.SupportPointSystem.Application.Interfaces
{
    public interface ICustomerAppService
    {
        Task RegisterCustomerAsync(CreateCustomerRequest request);
    }
}