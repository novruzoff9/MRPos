using Store.Application.Common.Validators;

namespace Store.Application.Features.Branches;

public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
{
    public CreateBranchCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Branch name is required and should not exceed 100 characters.");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty();
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description should not exceed 500 characters.");
        RuleFor(x => x.ServiceFee)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Service fee must be a positive value.");
        // 24 saat açıqdırsa saatlar verilməməlidir
        RuleFor(x => x.Opening)
            .Null()
            .When(x => x.Is24Hour)
            .WithMessage("Opening must be null when Is24Hour is true.");

        RuleFor(x => x.Closing)
            .Null()
            .When(x => x.Is24Hour)
            .WithMessage("Closing must be null when Is24Hour is true.");
        // 24 saat açıq deyilsə saatlar mütləq verilməlidir
        RuleFor(x => x.Opening)
            .NotNull()
            .When(x => !x.Is24Hour)
            .WithMessage("Opening time is required when Is24Hour is false.");

        RuleFor(c => c.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+994\d{2}-\d{3}-\d{2}-\d{2}$").WithMessage("PhoneNumber +994xx-xxx-xx-xx formatında olmalıdır");
        RuleFor(x => x.Closing)
            .NotNull()
            .When(x => !x.Is24Hour)
            .WithMessage("Closing time is required when Is24Hour is false.");

        RuleFor(x => x.Address)
            .NotNull()
            .WithMessage("Address is required.")
            .SetValidator(new AddressDtoValidator());
    }
}