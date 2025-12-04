namespace Store.Application.Features.Branches;

public class DeleteBranchCommandValidator : AbstractValidator<DeleteBranchCommand>
{
    public DeleteBranchCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Branch ID is required.");
    }
}