namespace FSI.SupportPointSystem.Application.Dtos.Customer.Request
{
    public record UpdateCustomerRequest(
        string CompanyName,
        string Cnpj,
        SupportPointSystem.Application.Dtos.Address.AddressDto Address, 
        double Latitude,
        double Longitude);
}
