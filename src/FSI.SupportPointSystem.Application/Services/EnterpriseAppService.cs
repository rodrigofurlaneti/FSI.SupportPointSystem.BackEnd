using System.Linq;
using System.Threading.Tasks;
using FSI.SupportPointSystem.Domain.Entities;          // Para encontrar a classe Enterprise
using FSI.SupportPointSystem.Domain.Interfaces.Services; // Para a interface do serviço externo
using FSI.SupportPointSystem.Application.Dtos;          // Onde está o seu EnterpriseDto
using FSI.SupportPointSystem.Application.Interfaces;    // Para a interface IEnterpriseAppService
using FSI.SupportPointSystem.Application.Extensions;    // Para o método .ToDto()
using FSI.SupportPoint.Application.Dtos.Enterprise;

namespace FSI.SupportPoint.Application.Services
{
    public class EnterpriseAppService : IEnterpriseAppService
    {
        private readonly IEnterpriseExternalService _externalService;

        public EnterpriseAppService(IEnterpriseExternalService externalService)
        {
            _externalService = externalService;
        }

        public async Task<EnterpriseDto> GetEnterpriseByCnpjAsync(string cnpj)
        {
            // O sinal de ? garante que se o cnpj vier nulo, não dê erro de referência
            string cleanCnpj = cnpj != null ? new string(cnpj.Where(char.IsDigit).ToArray()) : string.Empty;

            if (string.IsNullOrEmpty(cleanCnpj)) return null;

            var enterprise = await _externalService.GetByCnpjAsync(cleanCnpj);

            // Verifica se a busca retornou algo antes de tentar mapear
            return enterprise?.ToDto();
        }
    }
}