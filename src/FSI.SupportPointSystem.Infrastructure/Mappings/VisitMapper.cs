using FSI.SupportPoint.Domain.Entities;
using FSI.SupportPoint.Domain.ValueObjects;
using System.Dynamic;

namespace FSI.SupportPointSystem.Infrastructure.Mappings
{
    public static class VisitMapper
    {
        public static Visit ToDomain(dynamic row)
        {
            var checkinLoc = new Coordinates((decimal)row.latitude_captured, (decimal)row.longitude_captured);
            var visit = new Visit(
                (Guid)row.id,
                (Guid)row.seller_id,
                (Guid)row.customer_id,
                checkinLoc,
                (double)row.distance_meters,
                (DateTime)row.checkin_timestamp
            );
            if (row.checkout_timestamp != null)
            {
                var checkoutLoc = new Coordinates((decimal)row.checkout_latitude, (decimal)row.checkout_longitude);
                visit.PerformCheckout(checkoutLoc, (double)row.checkout_distance_meters);
            }
            return visit;
        }
    }
}

