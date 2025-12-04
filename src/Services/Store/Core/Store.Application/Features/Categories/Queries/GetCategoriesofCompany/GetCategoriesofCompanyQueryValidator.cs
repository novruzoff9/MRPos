namespace Store.Application.Features.Categories;

public class GetCategoriesofCompanyQueryValidator : AbstractValidator<GetCategoriesofCompany>
{
    public GetCategoriesofCompanyQueryValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("Company ID is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Company ID must be a valid GUID.");
    }
}