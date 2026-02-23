using FSI.SupportPoint.Application.Dtos.Customer.Request;
using FSI.SupportPointSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SupportPointSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerAppService _customerAppService;

        public CustomerController(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
        {
            await _customerAppService.RegisterCustomerAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = Guid.NewGuid() }, new { message = "Cliente cadastrado com sucesso." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerAppService.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _customerAppService.GetByIdAsync(id);
            return customer != null ? Ok(customer) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request)
        {
            await _customerAppService.UpdateCustomerAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _customerAppService.DeleteCustomerAsync(id);
            return NoContent();
        }
    }
}