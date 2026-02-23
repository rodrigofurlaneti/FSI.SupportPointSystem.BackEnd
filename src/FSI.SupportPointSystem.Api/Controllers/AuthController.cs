using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
namespace FSI.SupportPointSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        public AuthController(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] FSI.SupportPoint.Application.Dtos.Login.Request.LoginRequest request)
        {
            var user = await _userRepository.GetByCpfAsync(request.Cpf);

            if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("CPF ou senha inválidos.");

            var token = _authService.GenerateToken(user.Cpf, user.Role, user.Id);

            return Ok(new { Token = token, User = user.Name });
        }
    }
}