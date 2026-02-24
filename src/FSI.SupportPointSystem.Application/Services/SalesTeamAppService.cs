using FSI.SupportPointSystem.Application.Dtos.SalesTeam.Request;
using FSI.SupportPointSystem.Application.Dtos.SalesTeam.Response;
using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSI.SupportPointSystem.Application.Services
{
    public class SalesTeamAppService : ISalesTeamAppService
    {
        private readonly ISalesTeamRepository _salesTeamRepository;

        public SalesTeamAppService(ISalesTeamRepository salesTeamRepository)
        {
            _salesTeamRepository = salesTeamRepository;
        }

        public async Task CreateAsync(FSI.SupportPointSystem.Application.Dtos.SalesTeam.Request.CreateSalesTeamRequest request)
        {
            var salesTeam = new SalesTeam(request.Name, request.Description);
            await _salesTeamRepository.CreateAsync(salesTeam);
        }

        public async Task<IEnumerable<SalesTeamResponse>> GetAllAsync()
        {
            var teams = await _salesTeamRepository.GetAllAsync();
            return teams.Select(salesTeam => new SalesTeamResponse
            {
                Id = salesTeam.Id,
                Name = salesTeam.Name,
                Description = salesTeam.Description,
                Active = salesTeam.Active,
                CreatedAt = salesTeam.CreatedAt
            });
        }

        public async Task<SalesTeamResponse?> GetByIdAsync(Guid id)
        {
            var salesTeam = await _salesTeamRepository.GetByIdAsync(id);
            if (salesTeam == null) return null;

            return new SalesTeamResponse
            {
                Id = salesTeam.Id,
                Name = salesTeam.Name,
                Description = salesTeam.Description,
                Active = salesTeam.Active,
                CreatedAt = salesTeam.CreatedAt
            };
        }

        public async Task<SalesTeamWithSellersResponse?> GetTeamMembersAsync(Guid id)
        {
            var salesTeam = await _salesTeamRepository.GetByIdAsync(id);
            if (salesTeam == null) return null;

            return new SalesTeamWithSellersResponse
            {
                Id = salesTeam.Id,
                Name = salesTeam.Name,
                Description = salesTeam.Description,
                Active = salesTeam.Active,
                CreatedAt = salesTeam.CreatedAt,
                Sellers = new List<FSI.SupportPointSystem.Application.Dtos.Seller.Response.SellerResponse>()
            };
        }

        public async Task UpdateAsync(Guid id, UpdateSalesTeamRequest request)
        {
            var team = await _salesTeamRepository.GetByIdAsync(id);
            if (team == null)
                throw new Exception("Time de vendas não encontrado.");

            team.Update(request.Name, request.Description, request.Active);
            await _salesTeamRepository.UpdateAsync(team);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _salesTeamRepository.DeleteAsync(id);
        }
    }
}