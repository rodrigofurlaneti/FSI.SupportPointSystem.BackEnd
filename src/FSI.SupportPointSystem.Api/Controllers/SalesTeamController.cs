using FSI.SupportPointSystem.Application.Dtos.SalesTeam.Request;
using FSI.SupportPointSystem.Application.Dtos.SalesTeam.Response;
using FSI.SupportPointSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SupportPointSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class SalesTeamController : ControllerBase
    {
        private readonly ISalesTeamAppService _salesTeamAppService;

        public SalesTeamController(ISalesTeamAppService salesTeamAppService)
        {
            _salesTeamAppService = salesTeamAppService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] FSI.SupportPointSystem.Application.Dtos.SalesTeam.Request.CreateSalesTeamRequest request)
        {
            try
            {
                await _salesTeamAppService.CreateAsync(request);
                return Ok(new { message = "Time de vendas criado com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SalesTeamResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _salesTeamAppService.GetAllAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SalesTeamResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var team = await _salesTeamAppService.GetByIdAsync(id);
            return team != null ? Ok(team) : NotFound(new { message = "Time não encontrado." });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSalesTeamRequest request)
        {
            try
            {
                await _salesTeamAppService.UpdateAsync(id, request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _salesTeamAppService.DeleteAsync(id);
            return NoContent();
        }
    }
}