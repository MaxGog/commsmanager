using AutoMapper;
using FluentValidation;
using CommsManager.Application.DTOs.ArtistProfile;
using CommsManager.Application.Exceptions;
using CommsManager.Core.Entities;
using CommsManager.Core.Interfaces;

namespace CommsManager.Application.Services;

public interface IArtistProfileService
{
    Task<ArtistProfileResponseDto> CreateArtistProfileAsync(CreateArtistProfileDto dto);
    Task<ArtistProfileResponseDto> GetArtistProfileByIdAsync(Guid id);
    Task<List<ArtistProfileResponseDto>> GetAllArtistProfilesAsync();
    Task<ArtistProfileResponseDto> UpdateArtistProfileAsync(Guid id, UpdateArtistProfileDto dto);
    Task DeleteArtistProfileAsync(Guid id);
}

public class ArtistProfileService : IArtistProfileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateArtistProfileDto> _createValidator;

    public ArtistProfileService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateArtistProfileDto> createValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
    }

    public async Task<ArtistProfileResponseDto> CreateArtistProfileAsync(CreateArtistProfileDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());
            throw new Exceptions.ValidationException(errors);
        }

        var artistProfile = new ArtistProfile(dto.Name)
        {
            Description = dto.Description
        };

        await _unitOfWork.ArtistProfiles.AddAsync(artistProfile);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(artistProfile);
    }

    public async Task<ArtistProfileResponseDto> GetArtistProfileByIdAsync(Guid id)
    {
        var artistProfile = await _unitOfWork.ArtistProfiles.GetByIdAsync(id);
        if (artistProfile == null)
            throw new NotFoundException("ArtistProfile", id);

        return MapToResponse(artistProfile);
    }

    public async Task<List<ArtistProfileResponseDto>> GetAllArtistProfilesAsync()
    {
        var artistProfiles = await _unitOfWork.ArtistProfiles.GetAllAsync();
        return artistProfiles.Select(MapToResponse).ToList();
    }

    public async Task<ArtistProfileResponseDto> UpdateArtistProfileAsync(Guid id, UpdateArtistProfileDto dto)
    {
        var artistProfile = await _unitOfWork.ArtistProfiles.GetByIdAsync(id);
        if (artistProfile == null)
            throw new NotFoundException("ArtistProfile", id);

        if (!string.IsNullOrEmpty(dto.Name))
            artistProfile.Name = dto.Name;

        if (!string.IsNullOrEmpty(dto.Description))
            artistProfile.Description = dto.Description;

        await _unitOfWork.ArtistProfiles.UpdateAsync(artistProfile);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(artistProfile);
    }

    public async Task DeleteArtistProfileAsync(Guid id)
    {
        var artistProfile = await _unitOfWork.ArtistProfiles.GetByIdAsync(id);
        if (artistProfile == null)
            throw new NotFoundException("ArtistProfile", id);

        await _unitOfWork.ArtistProfiles.DeleteAsync(artistProfile);
        await _unitOfWork.SaveChangesAsync();
    }

    private ArtistProfileResponseDto MapToResponse(ArtistProfile artistProfile)
    {
        return new ArtistProfileResponseDto
        {
            Id = artistProfile.Id,
            Name = artistProfile.Name,
            Description = artistProfile.Description,
            CommissionCount = artistProfile.Commissions?.Count ?? 0,
            CreatedDate = artistProfile.CreatedDate
        };
    }
}
