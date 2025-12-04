namespace Store.Application.Features.Companies;

public class GetCompanyQueryValidator : AbstractValidator<GetCompanyQuery>
{
    public GetCompanyQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Company ID is required.");
    }
}