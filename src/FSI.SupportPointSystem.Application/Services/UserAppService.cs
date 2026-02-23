using FSI.SupportPoint.Application.Interfaces;
using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;

namespace FSI.SupportPointSystem.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;

        public UserAppService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetByCpfAsync(string cpf)
        {
            return await _userRepository.GetByCpfAsync(cpf);
        }
    }
}