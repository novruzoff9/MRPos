namespace Store.Application.Features.Products;

public class GetProductsByCategoryQueryValidator : AbstractValidator<GetProductsByCategoryQuery>
{
    public GetProductsByCategoryQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category ID is required.")
            .NotNull().WithMessage("Category ID cannot be null.");
    }
}
