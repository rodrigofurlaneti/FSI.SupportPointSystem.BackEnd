namespace FSI.SupportPointSystem.Application.Dtos.Customer.Request
{
    public record CreateCustomerRequest(string CompanyName, string Cnpj, double Latitude, double Longitude);
}