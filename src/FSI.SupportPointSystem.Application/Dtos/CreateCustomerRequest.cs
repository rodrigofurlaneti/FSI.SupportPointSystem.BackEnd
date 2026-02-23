namespace FSI.SupportPointSystem.Application.Dtos
{
    public record CreateCustomerRequest(string CompanyName, string Cnpj, decimal Latitude, decimal Longitude);
}