using FSI.SupportPointSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SupportPointSystem.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LeadController : ControllerBase
    {
        private readonly ILeadAppService _leadAppService;

        public LeadController(ILeadAppService leadAppService)
        {
            _leadAppService = leadAppService;
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableLeads()
        {
            var response = await _leadAppService.GetAvailableLeadsAsync();
            return Ok(response);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string newStatus)
        {
            await _leadAppService.UpdateStatusAsync(id, newStatus);
            return NoContent();
        }
    }
}