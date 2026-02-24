using FSI.SupportPointSystem.Application.Dtos.Address;

namespace FSI.SupportPointSystem.Application.Dtos.Customer.Response
{
    public record CustomerResponse(
        Guid Id,
        string CompanyName,
        string Cnpj,
        AddressDto Address, 
        string FullAddress, 
        double Latitude,
        double Longitude);
}