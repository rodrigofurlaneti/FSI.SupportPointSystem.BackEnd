using FSI.SupportPointSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.SupportPointSystem.Domain.Interfaces.Repositories
{
    public interface ISellerRepository
    {
        Task CreateAsync(Seller seller, string passwordHash);
        Task<Seller?> GetByIdAsync(Guid id);
        Task UpdateAsync(Seller seller);
        Task<IEnumerable<Seller>> GetAllAsync();
        Task DeleteAsync(Guid id);
        Task<Seller?> GetByCpfAsync(string cpf);
        Task<IEnumerable<Seller>> GetBySalesTeamIdAsync(Guid salesTeamId);
    }
}