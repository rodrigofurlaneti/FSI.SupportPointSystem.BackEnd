using System;
using System.Threading.Tasks;
using FSI.SupportPointSystem.Domain.ValueObjects;
namespace FSI.SupportPointSystem.Domain.Interfaces.Services
{
    public interface ILocationService
    {
        double CalculateDistanceInMeters(Coordinates point1, Coordinates point2);
    }
}