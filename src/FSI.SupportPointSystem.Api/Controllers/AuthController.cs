using FSI.SupportPointSystem.Application.Dtos;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SupportPointSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public AuthController(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 1. Busca usuário por CPF
            var user = await _userRepository.GetByCpfAsync(request.Cpf);

            // 2. Valida existência e senha
            if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "CPF ou senha inválidos." });
            }

            // 3. Gera o Token JWT
            var token = _authService.GenerateToken(user.Cpf, user.Role, user.Id);

            return Ok(new LoginResponse(
                Token: token,
                UserName: user.Name,
                Role: user.Role,
                UserId: user.Id,
                SellerId: user.Seller?.Id // Vinculado via 1:1 no domínio
            ));
        }
    }
}

