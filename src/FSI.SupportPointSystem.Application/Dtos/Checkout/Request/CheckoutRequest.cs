namespace FSI.SupportPoint.Application.Dtos.Checkout.Request
{
    public record CheckoutRequest(Guid SellerId, Guid CustomerId, decimal Latitude, decimal Longitude);
}