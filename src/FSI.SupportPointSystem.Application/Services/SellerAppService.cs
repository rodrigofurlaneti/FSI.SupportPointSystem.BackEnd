using FSI.SupportPointSystem.Application.DTOs;
using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
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
            var cpfValidado = new Cpf(request.Cpf).Value;
            var passwordHash = _authService.HashPassword(request.Password);
            await _sellerRepository.CreateAsync(
                cpfValidado,
                passwordHash,
                request.Name,
                request.Phone 
            );
        }
    }
}