using FSI.SupportPointSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SupportPointSystem.Domain.Interfaces.Services
{
    public interface IEnterpriseExternalService
    {
        Task<Enterprise> GetByCnpjAsync(string cnpj);
    }
}
