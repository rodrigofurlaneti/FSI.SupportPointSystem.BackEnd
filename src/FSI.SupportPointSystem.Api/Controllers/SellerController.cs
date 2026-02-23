using FSI.SupportPointSystem.Application.Dtos;
using FSI.SupportPointSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SupportPointSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class SellerController : ControllerBase
    {
        private readonly ISellerAppService _sellerAppService;

        public SellerController(ISellerAppService sellerAppService)
        {
            _sellerAppService = sellerAppService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSellerRequest request)
        {
            try
            {
                // A API apenas delega para a Application
                await _sellerAppService.RegisterSellerAsync(request);

                return Ok(new { message = "Vendedor cadastrado com sucesso." });
            }
            catch (Exception ex)
            {
                // Tratamento centralizado de erros
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}