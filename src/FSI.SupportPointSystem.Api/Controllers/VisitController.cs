using FSI.SupportPointSystem.Application.Dtos.Checkin.Request;
using FSI.SupportPointSystem.Application.Dtos.Checkout.Request;
using FSI.SupportPointSystem.Application.Dtos.Visit.Response;
using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SupportPointSystem.Api.Controllers
{
    [Authorize(Roles = "SELLER")]
    [ApiController]
    [Route("api/[controller]")]
    public class VisitController : ControllerBase
    {
        private readonly IVisitAppService _visitAppService;

        public VisitController(IVisitAppService visitAppService)
        {
            _visitAppService = visitAppService;
        }
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
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", detail = ex.Message });
            }
        }
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
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error processing checkout", detail = ex.Message });
            }
        }
    }
}