namespace Store.Application.Features.Branches;

public class UpdateBranchServiceFeeCommandValidator : AbstractValidator<UpdateBranchServiceFeeCommand>
{
    public UpdateBranchServiceFeeCommandValidator()
    {
        RuleFor(x => x.ServiceFee)
            .GreaterThanOrEqualTo(0).WithMessage("Service fee cannot be negative.");
    }
}
