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

            // Ajuste 1: Conversão segura de decimais para Coordinates
            var checkinLoc = new Coordinates(
                Convert.ToDecimal(row.LatitudeCaptured),
                Convert.ToDecimal(row.LongitudeCaptured)
            );

            // Ajuste 2: Conversão de IDs (String do MySQL para Guid)
            Guid visitId = row.Id is Guid guid ? guid : Guid.Parse(row.Id.ToString());
            Guid sellerId = row.SellerId is Guid sGuid ? sGuid : Guid.Parse(row.SellerId.ToString());
            Guid customerId = row.CustomerId is Guid cGuid ? cGuid : Guid.Parse(row.CustomerId.ToString());

            var visit = new Visit(
                visitId,
                sellerId,
                customerId,
                checkinLoc,
                Convert.ToDouble(row.DistanceMeters),
                (DateTime)row.CheckinTimestamp,
                row.SummaryCheckOut?.ToString()
            );

            // Ajuste 3: Verificação de nulidade compatível com MySQL (DBNull)
            if (row.CheckoutTimestamp != null && row.CheckoutTimestamp != DBNull.Value)
            {
                var checkoutLoc = new Coordinates(
                    Convert.ToDecimal(row.CheckoutLatitude),
                    Convert.ToDecimal(row.CheckoutLongitude)
                );

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