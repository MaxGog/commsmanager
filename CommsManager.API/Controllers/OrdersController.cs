using Microsoft.AspNetCore.Mvc;
using CommsManager.Application.DTOs.Order;
using CommsManager.Application.Services;

namespace CommsManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : BaseApiController
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Получить все заказы
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrders()
    {
        _logger.LogInformation("Fetching all orders");
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    /// <summary>
    /// Получить заказ по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetOrderById(Guid id)
    {
        _logger.LogInformation("Fetching order with ID: {OrderId}", id);
        var order = await _orderService.GetOrderByIdAsync(id);
        return Ok(order);
    }

    /// <summary>
    /// Получить заказы клиента
    /// </summary>
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrdersByCustomer(Guid customerId)
    {
        _logger.LogInformation("Fetching orders for customer: {CustomerId}", customerId);
        var orders = await _orderService.GetOrdersByCustomerAsync(customerId);
        return Ok(orders);
    }

    /// <summary>
    /// Получить заказы художника
    /// </summary>
    [HttpGet("artist/{artistId}")]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrdersByArtist(Guid artistId)
    {
        _logger.LogInformation("Fetching orders for artist: {ArtistId}", artistId);
        var orders = await _orderService.GetOrdersByArtistAsync(artistId);
        return Ok(orders);
    }

    /// <summary>
    /// Создать новый заказ
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] CreateOrderDto dto)
    {
        _logger.LogInformation("Creating new order for customer: {CustomerId}", dto.CustomerId);
        var order = await _orderService.CreateOrderAsync(dto);
        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }

    /// <summary>
    /// Обновить заказ
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<OrderResponseDto>> UpdateOrder(Guid id, [FromBody] UpdateOrderDto dto)
    {
        _logger.LogInformation("Updating order: {OrderId}", id);
        var order = await _orderService.UpdateOrderAsync(id, dto);
        return Ok(order);
    }

    /// <summary>
    /// Изменить статус заказа
    /// </summary>
    [HttpPatch("{id}/status")]
    public async Task<ActionResult<OrderResponseDto>> UpdateOrderStatus(Guid id, [FromBody] StatusUpdateRequest request)
    {
        _logger.LogInformation("Updating order status: {OrderId} to {Status}", id, request.Status);
        await _orderService.UpdateOrderStatusAsync(id, request.Status);
        var order = await _orderService.GetOrderByIdAsync(id);
        return Ok(order);
    }

    /// <summary>
    /// Удалить заказ
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(Guid id)
    {
        _logger.LogInformation("Deleting order: {OrderId}", id);
        await _orderService.DeleteOrderAsync(id);
        return NoContent();
    }
}

public record StatusUpdateRequest(string Status);
