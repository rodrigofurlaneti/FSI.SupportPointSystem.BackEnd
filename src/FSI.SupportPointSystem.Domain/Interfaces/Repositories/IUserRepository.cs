using System;
using System.Threading.Tasks;
using FSI.SupportPointSystem.Domain.Entities;
namespace FSI.SupportPointSystem.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByCpfAsync(string cpf);
        Task AddAsync(User user);
    }
}