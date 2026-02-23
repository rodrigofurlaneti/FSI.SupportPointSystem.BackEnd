namespace FSI.SupportPointSystem.Application.Dtos
{
    public record CheckinRequest(Guid SellerId, Guid CustomerId, decimal Latitude, decimal Longitude);
}
