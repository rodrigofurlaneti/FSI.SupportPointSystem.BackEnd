using FSI.SupportPointSystem.Application.Dtos;
using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SupportPointSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitController : ControllerBase
    {
        private readonly IVisitAppService _visitAppService;

        public VisitController(IVisitAppService visitAppService)
        {
            _visitAppService = visitAppService;
        }

        /// <summary>
        /// Realiza o Check-in do vendedor no cliente.
        /// </summary>
        [HttpPost("checkin")]
        [ProducesResponseType(typeof(VisitResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterCheckin([FromBody] CheckinRequest request)
        {
            try
            {
                var result = await _visitAppService.RegisterCheckinAsync(request);
                return Ok(result);
            }
            catch (DomainException ex)
            {
                // Erros de regra de negócio (Ex: fora do raio de 100m)
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", detail = ex.Message });
            }
        }

        /// <summary>
        /// Realiza o Check-out e finaliza a visita.
        /// </summary>
        [HttpPost("checkout")]
        [ProducesResponseType(typeof(VisitResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterCheckout([FromBody] CheckoutRequest request)
        {
            try
            {
                var result = await _visitAppService.RegisterCheckoutAsync(request);
                return Ok(result);
            }
            catch (BusinessRuleException ex)
            {
                // Erros de fluxo (Ex: Vendedor sem check-in ativo)
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error processing checkout", detail = ex.Message });
            }
        }
    }
}