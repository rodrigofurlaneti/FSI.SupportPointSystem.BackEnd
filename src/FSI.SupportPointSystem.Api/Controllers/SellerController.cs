using FSI.SupportPointSystem.Application.Dtos.Seller.Request;
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
            await _sellerAppService.RegisterSellerAsync(request);
            return Ok(new { message = "Vendedor cadastrado com sucesso." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sellers = await _sellerAppService.GetAllAsync();
            return Ok(sellers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var seller = await _sellerAppService.GetByIdAsync(id);
            return seller != null ? Ok(seller) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSellerRequest request)
        {
            await _sellerAppService.UpdateSellerAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _sellerAppService.DeleteSellerAsync(id);
            return NoContent();
        }
    }
}