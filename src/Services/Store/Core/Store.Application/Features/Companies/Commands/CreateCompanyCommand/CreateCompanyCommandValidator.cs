namespace Store.Application.Features.Companies;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(100).WithMessage("Company name must not exceed 100 characters.");
        RuleFor(c => c.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+994\d{2}-\d{3}-\d{2}-\d{2}$").WithMessage("Telefon nömrəi +994xx-xxx-xx-xx formatında olmalıdır");
        RuleFor(c => c.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        RuleFor(c => c.LogoUrl)
            .Must(uri => uri == null || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Logo URL must be a valid URL.");
    }
}