using FSI.SupportPoint.Application.Dtos.Enterprise;
using FSI.SupportPoint.Application.Services;        // Namespace do seu serviço
using FSI.SupportPointSystem.Application.Dtos;
using FSI.SupportPointSystem.Application.Interfaces; // Namespace da sua interface
using Microsoft.AspNetCore.Mvc;

namespace FSI.SupportPointSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Define a rota como api/enterprise
    public class EnterpriseController : ControllerBase
    {
        private readonly IEnterpriseAppService _enterpriseAppService;
        public EnterpriseController(IEnterpriseAppService enterpriseAppService)
        {
            _enterpriseAppService = enterpriseAppService;
        }

        /// <summary>
        /// Busca os dados de uma empresa pelo CNPJ em uma API externa.
        /// </summary>
        /// <param name="cnpj">Número do CNPJ (com ou sem formatação)</param>
        /// <returns>Objeto EnterpriseDto</returns>
        [HttpGet("{cnpj}")]
        [ProducesResponseType(typeof(EnterpriseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
            {
                return BadRequest("O CNPJ deve ser informado.");
            }

            var result = await _enterpriseAppService.GetEnterpriseByCnpjAsync(cnpj);

            if (result == null)
            {
                return NotFound(new { message = "Empresa não encontrada ou CNPJ inválido." });
            }

            return Ok(result);
        }
    }
}