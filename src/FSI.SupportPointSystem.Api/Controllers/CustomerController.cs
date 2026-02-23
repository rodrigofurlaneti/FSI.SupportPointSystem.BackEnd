using FSI.SupportPointSystem.Application.Dtos;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SupportPointSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")] // Apenas Admins gerenciam clientes
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
            return Ok(new { message = "Cliente processado pela camada de aplicação." });
        }
    }
}