using FSI.SupportPointSystem.Application.Dtos.Lead.Response;
using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;

namespace FSI.SupportPointSystem.Application.Services
{
    public class LeadAppService : ILeadAppService
    {
        private readonly ILeadRepository _leadRepository;

        public LeadAppService(ILeadRepository leadRepository)
        {
            _leadRepository = leadRepository;
        }

        public async Task<IEnumerable<LeadResponse>> GetAvailableLeadsAsync()
        {
            var leads = await _leadRepository.GetAllAvailableAsync();

            return leads.Select(l => new LeadResponse
            {
                Id = l.Id,
                CompanyName = l.CompanyName,
                ContactName = l.ContactName,
                Phone = l.Phone,
                Address = l.Address,
                OpportunityDescription = l.OpportunityDescription,
                Status = l.Status
            });
        }

        public async Task UpdateStatusAsync(Guid id, string newStatus)
        {
            // Aqui poderiam entrar regras de negócio antes de persistir
            await _leadRepository.UpdateStatusAsync(id, newStatus);
        }
    }
}