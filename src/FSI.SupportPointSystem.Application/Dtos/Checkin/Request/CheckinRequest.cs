namespace FSI.SupportPoint.Application.Dtos.Checkin.Request
{
    public record CheckinRequest(Guid SellerId, Guid CustomerId, decimal Latitude, decimal Longitude);
}
