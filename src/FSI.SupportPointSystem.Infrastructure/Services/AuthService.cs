using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FSI.SupportPointSystem.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace FSI.SupportPointSystem.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string cpf, string role, Guid userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Busca a chave do appsettings.json que configuramos para o ambiente RDS
            var secret = _configuration["Jwt:Secret"] ?? "Chave_Muito_Longa_E_Secreta_Para_FSI_2026";
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, cpf),
                    new Claim(ClaimTypes.Role, role),
                    // Mantemos o ToString() para que o ID no JWT seja idêntico ao CHAR(36) no MySQL
                    new Claim("UserId", userId.ToString())
                }),
                // Ajuste: Tempo de expiração vindo do appsettings.json ou padrão de 8 horas
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"] ?? "8")),

                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            // O BCrypt é agnóstico ao banco, então continuará funcionando 
            // perfeitamente com o hash armazenado no VARCHAR(255) do MySQL.
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}