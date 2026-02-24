using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.ValueObjects;
using System;

namespace FSI.SupportPointSystem.Infrastructure.Mappings
{
    public static class VisitMapper
    {
        public static Visit ToDomain(dynamic row)
        {
            if (row == null) return null;
            var checkinLoc = new Coordinates((decimal)row.LatitudeCaptured, (decimal)row.LongitudeCaptured);
            var visit = new Visit(
                (Guid)row.Id,
                (Guid)row.SellerId,
                (Guid)row.CustomerId,
                checkinLoc,
                Convert.ToDouble(row.DistanceMeters), 
                (DateTime)row.CheckinTimestamp,
                (string)row.SummaryCheckOut
            );
            if (row.CheckoutTimestamp != null)
            {
                var checkoutLoc = new Coordinates((decimal)row.CheckoutLatitude, (decimal)row.CheckoutLongitude);
                visit.PerformCheckout(
                    checkoutLoc,
                    Convert.ToDouble(row.CheckoutDistanceMeters),
                    row.SummaryCheckOut?.ToString() ?? string.Empty 
                );
            }

            return visit;
        }
    }
}