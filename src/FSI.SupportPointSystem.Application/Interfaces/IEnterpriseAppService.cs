using FSI.SupportPoint.Application.Dtos.Enterprise;
using FSI.SupportPointSystem.Application.Dtos;
using System.Threading.Tasks;

namespace FSI.SupportPointSystem.Application.Interfaces
{
    public interface IEnterpriseAppService
    {
        Task<EnterpriseDto> GetEnterpriseByCnpjAsync(string cnpj);
    }
}