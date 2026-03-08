using FSI.SupportPointSystem.Application.Dtos.Lead.Response;

namespace FSI.SupportPointSystem.Application.Interfaces
{
    public interface ILeadAppService
    {
        Task<IEnumerable<LeadResponse>> GetAvailableLeadsAsync();
        Task UpdateStatusAsync(Guid id, string newStatus);
    }
}