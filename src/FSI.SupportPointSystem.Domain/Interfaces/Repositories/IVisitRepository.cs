using System;
using System.Threading.Tasks;
using FSI.SupportPointSystem.Domain.Entities;
namespace FSI.SupportPointSystem.Domain.Interfaces.Repository
{
    public interface IVisitRepository
    {
        Task<Visit?> GetActiveVisitBySellerIdAsync(Guid sellerId);
        Task SaveCheckinAsync(Visit visit); 
        Task SaveCheckoutAsync(Visit visit);
    }
}