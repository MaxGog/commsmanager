using FluentValidation;
using CommsManager.Application.DTOs.Order;

namespace CommsManager.Application.Validators.Order;

public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название заказа обязательно")
            .MaximumLength(200).WithMessage("Название не может быть больше 200 символов");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("ID клиента обязателен");

        RuleFor(x => x.ArtistId)
            .NotEmpty().WithMessage("ID художника обязателен");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Цена должна быть больше 0");

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.Now).WithMessage("Срок должен быть в будущем");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Валюта обязательна");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Описание не может быть больше 2000 символов")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
