namespace CommsManager.Application.DTOs.Customer;

public class UpdateCustomerDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}
