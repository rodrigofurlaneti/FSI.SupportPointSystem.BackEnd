namespace FSI.SupportPoint.Application.Dtos.Customer.Response
{
    public record CustomerResponse(Guid Id, string CompanyName, string Cnpj, string Address, double Latitude, double Longitude);
}