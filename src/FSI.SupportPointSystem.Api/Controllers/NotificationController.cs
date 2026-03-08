using FSI.SupportPointSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FSI.SupportPointSystem.Api.Controllers
{
    [Authorize(Roles = "SELLER")]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IVisitAppService _visitAppService;

        public NotificationController(IVisitAppService visitAppService)
        {
            _visitAppService = visitAppService;
        }

        /// <summary>
        /// Verifica se o vendedor possui algum check-in sem check-out realizado.
        /// </summary>
        /// <param name="sellerId">ID do Vendedor (Guid/Char36)</param>
        [HttpGet("pending-status/{sellerId}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPendingStatus(string sellerId)
        {
            try
            {
                // Chama a implementação que executa a Procedure no Banco de Dados
                bool hasPending = await _visitAppService.HasPendingCheckinAsync(sellerId);

                return Ok(new
                {
                    hasPending = hasPending,
                    message = hasPending ? "Você possui um check-in em aberto." : "Nenhuma pendência encontrada.",
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                // Logar o erro aqui (ex: ILogger)
                return StatusCode(500, new
                {
                    message = "Erro ao verificar notificações pendentes",
                    detail = ex.Message
                });
            }
        }
    }
}