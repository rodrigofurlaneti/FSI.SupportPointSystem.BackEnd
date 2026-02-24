using FSI.SupportPointSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.SupportPointSystem.Domain.Interfaces.Repositories
{
    public interface ISalesTeamRepository
    {
        Task CreateAsync(SalesTeam salesTeam);
        Task<SalesTeam?> GetByIdAsync(Guid id);
        Task UpdateAsync(SalesTeam salesTeam);
        Task<IEnumerable<SalesTeam>> GetAllAsync();
        Task DeleteAsync(Guid id);
        Task<SalesTeam?> GetByNameAsync(string name);
        Task<SalesTeam?> GetByIdWithSellersAsync(Guid id);
    }
}