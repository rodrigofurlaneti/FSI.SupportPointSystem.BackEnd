using FSI.SupportPointSystem.Application.Dtos.SalesTeam.Request;
using FSI.SupportPointSystem.Application.Dtos.SalesTeam.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.SupportPointSystem.Application.Interfaces
{
    public interface ISalesTeamAppService
    {
        Task CreateAsync(FSI.SupportPointSystem.Application.Dtos.SalesTeam.Request.CreateSalesTeamRequest request);
        Task<IEnumerable<SalesTeamResponse>> GetAllAsync();
        Task<SalesTeamResponse?> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, UpdateSalesTeamRequest request);
        Task DeleteAsync(Guid id);
        Task<SalesTeamWithSellersResponse?> GetTeamMembersAsync(Guid id);
    }
}