using System;
using FSI.SupportPointSystem.Domain.Exceptions;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.ValueObjects;
namespace FSI.SupportPointSystem.Domain.Entities
{
    public class Visit
    {
        public Guid Id { get; private set; }
        public Guid SellerId { get; private set; }
        public Guid CustomerId { get; private set; }
        public Coordinates CheckinLocation { get; private set; }
        public DateTime CheckinTimestamp { get; private set; }
        public double CheckinDistance { get; private set; }
        public Coordinates? CheckoutLocation { get; private set; }
        public DateTime? CheckoutTimestamp { get; private set; }
        public double? CheckoutDistance { get; private set; }
        public int? DurationMinutes { get; private set; }
        public Visit(Guid sellerId, Guid customerId, Coordinates location, double distance)
        {
            if (distance > 100)
                throw new InvalidOperationException("Vendedor fora do raio de 100m permitido.");

            Id = Guid.NewGuid();
            SellerId = sellerId;
            CustomerId = customerId;
            CheckinLocation = location;
            CheckinDistance = distance;
            CheckinTimestamp = DateTime.UtcNow;
        }
        public void PerformCheckout(Coordinates location, double distance)
        {
            if (distance > 100)
                throw new InvalidOperationException("Vendedor fora do raio de 100m para checkout.");

            CheckoutLocation = location;
            CheckoutDistance = distance;
            CheckoutTimestamp = DateTime.UtcNow;
            DurationMinutes = (int)(CheckoutTimestamp.Value - CheckinTimestamp).TotalMinutes;
        }
    }
}
