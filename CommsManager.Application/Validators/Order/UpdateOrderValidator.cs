using FluentValidation;
using CommsManager.Application.DTOs.Order;

namespace CommsManager.Application.Validators.Order;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderDto>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Название не может быть больше 200 символов")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Цена должна быть больше 0")
            .When(x => x.Price.HasValue && x.Price > 0);

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.Now).WithMessage("Срок должен быть в будущем")
            .When(x => x.Deadline.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Описание не может быть больше 2000 символов")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Status)
            .Must(s => s == null || new[] { "New", "InProgress", "Completed", "Cancelled" }.Contains(s))
            .WithMessage("Некорректный статус заказа")
            .When(x => !string.IsNullOrEmpty(x.Status));
    }
}
