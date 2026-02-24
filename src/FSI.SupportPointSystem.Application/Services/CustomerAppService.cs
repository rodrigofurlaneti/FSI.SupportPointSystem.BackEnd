using FSI.SupportPointSystem.Application.Dtos.Address;
using FSI.SupportPointSystem.Application.Dtos.Customer.Request;
using FSI.SupportPointSystem.Application.Dtos.Customer.Response;
using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var address = new Address(
                request.Address.ZipCode,
                request.Address.Street,
                request.Address.Number,
                request.Address.Neighborhood,
                request.Address.City,
                request.Address.State,
                request.Address.Complement
            );
            var customer = new Customer(
                Guid.NewGuid(),
                request.CompanyName,
                cnpj,
                address,
                location
            );
            await _customerRepository.UpsertAsync(customer);
        }
        public async Task<IEnumerable<CustomerResponse>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(c => MapToResponse(c)).ToList();
        }
        public async Task<CustomerResponse?> GetByIdAsync(Guid id)
        {
            var c = await _customerRepository.GetByIdAsync(id);
            return c == null ? null : MapToResponse(c);
        }
        public async Task UpdateCustomerAsync(Guid id, UpdateCustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) throw new Exception("Cliente não encontrado.");
            var newLocation = new Coordinates((decimal)request.Latitude, (decimal)request.Longitude);
            var newAddress = new Address(
                request.Address.ZipCode,
                request.Address.Street,
                request.Address.Number,
                request.Address.Neighborhood,
                request.Address.City,
                request.Address.State,
                request.Address.Complement
            );
            customer.UpdateCompanyName(request.CompanyName);
            customer.UpdateLocation(newLocation);
            customer.UpdateAddress(newAddress); 
            await _customerRepository.UpdateAsync(customer);
        }
        public async Task DeleteCustomerAsync(Guid id)
        {
            await _customerRepository.DeleteAsync(id);
        }
        private static CustomerResponse MapToResponse(Customer customer)
        {
            var addressDto = new AddressDto(
                customer.Address.ZipCode,
                customer.Address.Street,
                customer.Address.Number,
                customer.Address.Complement,
                customer.Address.Neighborhood,
                customer.Address.City,
                customer.Address.State
            );
            return new CustomerResponse(
                customer.Id,
                customer.CompanyName,
                customer.Cnpj.Value,
                addressDto,
                customer.Address.ToString(), 
                (double)customer.LocationTarget.Latitude,
                (double)customer.LocationTarget.Longitude
            );
        }
    }
}