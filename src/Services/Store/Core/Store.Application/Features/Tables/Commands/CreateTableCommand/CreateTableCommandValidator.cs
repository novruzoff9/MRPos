namespace Store.Application.Features.Tables;

public class CreateTableCommandValidator : AbstractValidator<CreateTableCommand>
{
    public CreateTableCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Table name is required.")
            .MaximumLength(100).WithMessage("Table name must not exceed 100 characters.");
        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than zero.");
        RuleFor(x => x.Deposit)
            .GreaterThanOrEqualTo(0).When(x => x.Deposit.HasValue).WithMessage("Deposit must be greater than or equal to zero.");
    }
}