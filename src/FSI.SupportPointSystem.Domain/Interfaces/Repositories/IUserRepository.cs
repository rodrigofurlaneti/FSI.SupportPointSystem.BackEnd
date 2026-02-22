using System;
using System.Threading.Tasks;
using FSI.SupportPoint.Domain.Entities;
namespace FSI.SupportPoint.Domain.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByCpfAsync(string cpf);
        Task AddAsync(User user);
    }
}