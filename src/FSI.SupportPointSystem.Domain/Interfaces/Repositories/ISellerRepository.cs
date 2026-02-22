using System;
using System.Threading.Tasks;
using FSI.SupportPoint.Domain.Entities;
namespace FSI.SupportPoint.Domain.Interfaces.Repository
{
    public interface ISellerRepository
    {
        Task AddWithUserAsync(Seller seller, string passwordHash); // Chama SpCreateSeller
        Task<Seller?> GetByIdAsync(Guid id);
    }
}
