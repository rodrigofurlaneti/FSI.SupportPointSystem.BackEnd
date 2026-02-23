using FSI.SupportPointSystem.Domain.Entities;
namespace FSI.SupportPoint.Application.Interfaces
{
    public interface IUserAppService
    {
        Task<User?> GetByCpfAsync(string cpf);
    }
}
