namespace CommsManager.Application.DTOs.Order;

public class UpdateOrderDto
{
    public string? Title { get; set; }
    public decimal? Price { get; set; }
    public DateTime? Deadline { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
}
