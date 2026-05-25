namespace CommsManager.Application.DTOs.ArtistProfile;

public class ArtistProfileResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int CommissionCount { get; set; }
    public DateTime CreatedDate { get; set; }
}
