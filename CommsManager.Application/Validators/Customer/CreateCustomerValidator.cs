using FluentValidation;
using CommsManager.Application.DTOs.Customer;

namespace CommsManager.Application.Validators.Customer;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя клиента обязательно")
            .MaximumLength(100).WithMessage("Имя не может быть больше 100 символов");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Некорректный формат email")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Номер телефона не может быть больше 20 символов")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Описание не может быть больше 1000 символов")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
