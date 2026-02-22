using SupportPoint.Domain.ValueObjects;
namespace FSI.SupportPoint.Domain.Services
{
    public class LocationService
    {
        private const double EarthRadiusKm = 6371.0;
        public double CalculateDistanceInMeters(Coordinates point1, Coordinates point2)
        {
            var dLat = ToRadians((double)(point2.Latitude - point1.Latitude));
            var dLon = ToRadians((double)(point2.Longitude - point1.Longitude));
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians((double)point1.Latitude)) * Math.Cos(ToRadians((double)point2.Latitude)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return (EarthRadiusKm * c) * 1000; 
        }
        private double ToRadians(double angle) => Math.PI * angle / 180.0;
    }
}