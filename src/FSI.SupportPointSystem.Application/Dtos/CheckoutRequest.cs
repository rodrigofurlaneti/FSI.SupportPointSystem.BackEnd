namespace FSI.SupportPointSystem.Application.Dtos
{
    public record CheckoutRequest(Guid SellerId, Guid CustomerId, decimal Latitude, decimal Longitude);
}