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
            .NotEmpty()
            .MaximumLength(15)
            .WithMessage("Phone number is required and should not exceed 15 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description should not exceed 500 characters.");
        RuleFor(x => x.ServiceFee)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Service fee must be a positive value.");
        RuleFor(x => x.Address)
            .NotNull()
            .WithMessage("Address is required.")
            .SetValidator(new AddressDtoValidator());
    }
}