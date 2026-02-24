using FSI.SupportPointSystem.Domain.Entities;
namespace FSI.SupportPointSystem.Application.Interfaces
{
    public interface IUserAppService
    {
        Task<User?> GetByCpfAsync(string cpf);
    }
}
