using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Application.Dtos;
using FSI.SupportPointSystem.Domain.Interfaces.Repository;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.ValueObjects;

namespace FSI.SupportPointSystem.Application.Services
{
    public class CustomerAppService : ICustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerAppService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task RegisterCustomerAsync(CreateCustomerRequest request)
        {
            var cnpj = new Cnpj(request.Cnpj);
            var location = new Coordinates(request.Latitude, request.Longitude);
            var customer = new Customer(request.CompanyName, cnpj, location);
            await _customerRepository.UpsertAsync(customer);
        }
    }
}