using Microsoft.AspNetCore.Mvc;
using CommsManager.Application.DTOs.ArtistProfile;
using CommsManager.Application.Services;

namespace CommsManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistProfilesController : BaseApiController
{
    private readonly IArtistProfileService _artistProfileService;
    private readonly ILogger<ArtistProfilesController> _logger;

    public ArtistProfilesController(IArtistProfileService artistProfileService, ILogger<ArtistProfilesController> logger)
    {
        _artistProfileService = artistProfileService;
        _logger = logger;
    }

    /// <summary>
    /// Получить все профили художников
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArtistProfileResponseDto>>> GetAllArtistProfiles()
    {
        _logger.LogInformation("Fetching all artist profiles");
        var profiles = await _artistProfileService.GetAllArtistProfilesAsync();
        return Ok(profiles);
    }

    /// <summary>
    /// Получить профиль художника по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ArtistProfileResponseDto>> GetArtistProfileById(Guid id)
    {
        _logger.LogInformation("Fetching artist profile with ID: {ArtistId}", id);
        var profile = await _artistProfileService.GetArtistProfileByIdAsync(id);
        return Ok(profile);
    }

    /// <summary>
    /// Создать новый профиль художника
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ArtistProfileResponseDto>> CreateArtistProfile([FromBody] CreateArtistProfileDto dto)
    {
        _logger.LogInformation("Creating new artist profile: {ArtistName}", dto.Name);
        var profile = await _artistProfileService.CreateArtistProfileAsync(dto);
        return CreatedAtAction(nameof(GetArtistProfileById), new { id = profile.Id }, profile);
    }

    /// <summary>
    /// Обновить профиль художника
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ArtistProfileResponseDto>> UpdateArtistProfile(Guid id, [FromBody] UpdateArtistProfileDto dto)
    {
        _logger.LogInformation("Updating artist profile: {ArtistId}", id);
        var profile = await _artistProfileService.UpdateArtistProfileAsync(id, dto);
        return Ok(profile);
    }

    /// <summary>
    /// Удалить профиль художника
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteArtistProfile(Guid id)
    {
        _logger.LogInformation("Deleting artist profile: {ArtistId}", id);
        await _artistProfileService.DeleteArtistProfileAsync(id);
        return NoContent();
    }
}
