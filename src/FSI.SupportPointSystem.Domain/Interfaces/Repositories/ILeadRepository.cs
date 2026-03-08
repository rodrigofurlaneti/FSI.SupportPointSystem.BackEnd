using FSI.SupportPointSystem.Domain.Entities;

namespace FSI.SupportPointSystem.Domain.Interfaces.Repositories
{
    public interface ILeadRepository
    {
        Task<IEnumerable<Lead>> GetAllAvailableAsync();
        Task<Lead?> GetByIdAsync(Guid id);
        Task UpdateStatusAsync(Guid id, string status);
    }
}