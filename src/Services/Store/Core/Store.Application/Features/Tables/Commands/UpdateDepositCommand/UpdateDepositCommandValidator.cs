namespace Store.Application.Features.Tables;

public class UpdateDepositCommandValidator : AbstractValidator<UpdateDepositCommand>
{
    public UpdateDepositCommandValidator()
    {
        RuleFor(x => x.TableId)
            .NotEmpty().WithMessage("Table ID is required.");
        RuleFor(x => x.Deposit)
            .GreaterThanOrEqualTo(0).WithMessage("Deposit must be greater than or equal to zero.");
    }
}
