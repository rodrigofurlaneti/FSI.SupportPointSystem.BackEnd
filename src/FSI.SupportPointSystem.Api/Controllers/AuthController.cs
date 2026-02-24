using FSI.SupportPointSystem.Application.Dtos.Login.Request;
using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Domain.Interfaces.Services;
using FSI.SupportPointSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserAppService _userAppService;
    private readonly IAuthService _authService;

    public AuthController(IUserAppService userAppService, IAuthService authService)
    {
        _userAppService = userAppService;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var user = await _userAppService.GetByCpfAsync(request.Cpf);
        var hashGerado = _authService.HashPassword(request.Password);//test
        var auth = _authService.VerifyPassword(request.Password, user.PasswordHash);
        if (user == null || !auth)
            return Unauthorized("CPF ou senha inválidos.");
        var token = _authService.GenerateToken(user.Cpf, user.Role, user.Id);
        return Ok(new { token });
    }
}