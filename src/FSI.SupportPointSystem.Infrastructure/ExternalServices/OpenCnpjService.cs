using System.Net.Http.Json;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Services; 

namespace FSI.SupportPointSystem.Infrastructure.ExternalServices
{
    public class OpenCnpjService : IEnterpriseExternalService
    {
        private readonly HttpClient _httpClient;

        public OpenCnpjService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Enterprise> GetByCnpjAsync(string cnpj)
        {
            try
            {
                string cleanCnpj = new string(cnpj.Where(char.IsDigit).ToArray());
                var enterprise = await _httpClient.GetFromJsonAsync<Enterprise>(cleanCnpj);

                return enterprise;
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}