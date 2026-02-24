namespace FSI.SupportPointSystem.Application.Dtos.Address
{
    public record AddressDto(
        string ZipCode,
        string Street,
        string Number,
        string Complement,
        string Neighborhood,
        string City,
        string State);
}