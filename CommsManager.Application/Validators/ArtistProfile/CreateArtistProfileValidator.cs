using FluentValidation;
using CommsManager.Application.DTOs.ArtistProfile;

namespace CommsManager.Application.Validators.ArtistProfile;

public class CreateArtistProfileValidator : AbstractValidator<CreateArtistProfileDto>
{
    public CreateArtistProfileValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя художника обязательно")
            .MaximumLength(100).WithMessage("Имя не может быть больше 100 символов");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Описание не может быть больше 2000 символов")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
