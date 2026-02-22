using System;
using System.Threading.Tasks;
using FSI.SupportPoint.Domain.ValueObjects;
namespace FSI.SupportPoint.Domain.Interfaces.Services
{
    public interface ILocationService
    {
        double CalculateDistanceInMeters(Coordinates point1, Coordinates point2);
    }
}