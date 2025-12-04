using Store.Application.Common.Models.Branch;

namespace Store.Application.Common.Validators;

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Street is required.");
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.");
        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage("State is required.");
        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .WithMessage("Zip code is required.");
    }
}
