using System;
namespace FSI.SupportPointSystem.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        string GenerateToken(string cpf, string role, Guid userId);
        bool VerifyPassword(string password, string passwordHash);
        string HashPassword(string password);
    }
}