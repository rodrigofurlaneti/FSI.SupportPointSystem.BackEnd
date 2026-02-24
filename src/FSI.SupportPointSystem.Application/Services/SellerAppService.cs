using FSI.SupportPointSystem.Application.Dtos.Seller.Request;
using FSI.SupportPointSystem.Application.Dtos.Seller.Response;
using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Domain.Interfaces.Services;
using FSI.SupportPointSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSI.SupportPointSystem.Application.Services
{
    public class SellerAppService : ISellerAppService
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IAuthService _authService;
        public SellerAppService(ISellerRepository sellerRepository, IAuthService authService)
        {
            _sellerRepository = sellerRepository;
            _authService = authService;
        }
        public async Task RegisterSellerAsync(CreateSellerRequest request)
        {
            var passwordHash = _authService.HashPassword(request.Password);
            var seller = new Seller(
                Guid.NewGuid(),
                request.Name,
                new Cpf(request.Cpf),
                request.Email,
                request.Phone
            );
            await _sellerRepository.CreateAsync(seller, passwordHash);
        }
        public async Task<IEnumerable<SellerResponse>> GetAllAsync()
        {
            var sellers = await _sellerRepository.GetAllAsync();
            return sellers.Select(s => new SellerResponse(
                s.Id,
                s.Name,
                s.Cpf.Value,
                s.Email,
                s.Phone
            ));
        }
        public async Task<SellerResponse?> GetByIdAsync(Guid id)
        {
            var s = await _sellerRepository.GetByIdAsync(id);
            if (s == null) return null;
            return new SellerResponse(s.Id, s.Name, s.Cpf.Value, s.Email, s.Phone);
        }
        public async Task UpdateSellerAsync(Guid id, UpdateSellerRequest request)
        {
            var seller = await _sellerRepository.GetByIdAsync(id);
            if (seller == null)
                throw new Exception("Vendedor não encontrado.");
            seller.UpdateDetails(
                request.Name,
                request.Email,
                request.Phone,
                request.IsActive 
            );

            await _sellerRepository.UpdateAsync(seller);
        }
        public async Task DeleteSellerAsync(Guid id)
        {
            await _sellerRepository.DeleteAsync(id);
        }
    }
}