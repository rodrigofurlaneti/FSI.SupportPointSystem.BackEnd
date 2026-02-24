using FSI.SupportPointSystem.Application.Dtos.Address;

namespace FSI.SupportPointSystem.Application.Dtos.Customer.Request
{
    public record CreateCustomerRequest(string CompanyName, string Cnpj, AddressDto Address, double Latitude, double Longitude);
}