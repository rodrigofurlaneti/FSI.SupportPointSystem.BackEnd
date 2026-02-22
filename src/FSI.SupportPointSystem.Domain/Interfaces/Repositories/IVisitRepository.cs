using System;
using System.Threading.Tasks;
using FSI.SupportPoint.Domain.Entities;
namespace FSI.SupportPoint.Domain.Interfaces.Repository
{
    public interface IVisitRepository
    {
        Task<Visit?> GetActiveVisitBySellerIdAsync(Guid sellerId);
        Task SaveCheckinAsync(Visit visit); 
        Task SaveCheckoutAsync(Visit visit);
    }
}