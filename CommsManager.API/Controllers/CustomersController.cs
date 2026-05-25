using Microsoft.AspNetCore.Mvc;
using CommsManager.Application.DTOs.Customer;
using CommsManager.Application.Services;

namespace CommsManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : BaseApiController
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    /// <summary>
    /// Получить всех клиентов
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAllCustomers()
    {
        _logger.LogInformation("Fetching all customers");
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    /// <summary>
    /// Получить клиента по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> GetCustomerById(Guid id)
    {
        _logger.LogInformation("Fetching customer with ID: {CustomerId}", id);
        var customer = await _customerService.GetCustomerByIdAsync(id);
        return Ok(customer);
    }

    /// <summary>
    /// Создать нового клиента
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> CreateCustomer([FromBody] CreateCustomerDto dto)
    {
        _logger.LogInformation("Creating new customer: {CustomerName}", dto.Name);
        var customer = await _customerService.CreateCustomerAsync(dto);
        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
    }

    /// <summary>
    /// Обновить клиента
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> UpdateCustomer(Guid id, [FromBody] UpdateCustomerDto dto)
    {
        _logger.LogInformation("Updating customer: {CustomerId}", id);
        var customer = await _customerService.UpdateCustomerAsync(id, dto);
        return Ok(customer);
    }

    /// <summary>
    /// Удалить клиента
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(Guid id)
    {
        _logger.LogInformation("Deleting customer: {CustomerId}", id);
        await _customerService.DeleteCustomerAsync(id);
        return NoContent();
    }
}
