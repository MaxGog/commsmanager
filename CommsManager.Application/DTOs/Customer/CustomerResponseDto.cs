namespace CommsManager.Application.DTOs.Customer;

public class CustomerResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Description { get; set; }
    public int OrderCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}
