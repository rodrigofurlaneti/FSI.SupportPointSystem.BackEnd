namespace FSI.SupportPointSystem.Application.Dtos.Checkout.Request
{
    public record CheckoutRequest(Guid SellerId, Guid CustomerId, decimal Latitude, decimal Longitude, string SummaryCheckOut);
}