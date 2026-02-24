using FSI.SupportPointSystem.Application.Dtos.Customer.Response;
using FSI.SupportPointSystem.Application.Dtos.Customer.Request;
using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
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
            var location = new Coordinates((decimal)request.Latitude, (decimal)request.Longitude);
            var customer = new Customer(
                Guid.NewGuid(), 
                request.CompanyName,
                cnpj,
                location
            );

            await _customerRepository.UpsertAsync(customer);
        }

        public async Task<IEnumerable<CustomerResponse>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            return customers.Select(c => new CustomerResponse(
                c.Id,
                c.CompanyName,
                c.Cnpj.Value,
                "Endereço não mapeado",
                (double)c.LocationTarget.Latitude,
                (double)c.LocationTarget.Longitude));
        }

        public async Task<CustomerResponse?> GetByIdAsync(Guid id)
        {
            var c = await _customerRepository.GetByIdAsync(id);
            if (c == null) return null;
            return new CustomerResponse(
                c.Id,
                c.CompanyName,
                c.Cnpj.Value,
                "Endereço não mapeado",
                (double)c.LocationTarget.Latitude,
                (double)c.LocationTarget.Longitude);
        }
        public async Task UpdateCustomerAsync(Guid id, UpdateCustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) throw new Exception("Cliente não encontrado.");
            var newLocation = new Coordinates((decimal)request.Latitude, (decimal)request.Longitude);
            customer.UpdateCompanyName(request.CompanyName);
            customer.UpdateLocation(newLocation);
            await _customerRepository.UpdateAsync(customer);
        }
        public async Task DeleteCustomerAsync(Guid id)
        {
            await _customerRepository.DeleteAsync(id);
        }
    }
}