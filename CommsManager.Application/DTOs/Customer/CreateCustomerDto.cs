namespace CommsManager.Application.DTOs.Customer;

public class CreateCustomerDto
{
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Description { get; set; }
}
