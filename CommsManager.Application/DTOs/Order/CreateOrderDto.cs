namespace CommsManager.Application.DTOs.Order;

public class CreateOrderDto
{
    public required string Title { get; set; }
    public required Guid CustomerId { get; set; }
    public required Guid ArtistId { get; set; }
    public required decimal Price { get; set; }
    public string Currency { get; set; } = "RUB";
    public required DateTime Deadline { get; set; }
    public string? Description { get; set; }
}
