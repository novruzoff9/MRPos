namespace Store.Application.Features.Products;

public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    public GetProductQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product ID is required.")
            .NotNull().WithMessage("Product ID cannot be null.");
    }
}
