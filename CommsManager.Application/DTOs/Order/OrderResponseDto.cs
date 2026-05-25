namespace CommsManager.Application.DTOs.Order;

public class OrderResponseDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string CustomerName { get; set; }
    public required string ArtistName { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "RUB";
    public required string Status { get; set; }
    public DateTime Deadline { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
}
