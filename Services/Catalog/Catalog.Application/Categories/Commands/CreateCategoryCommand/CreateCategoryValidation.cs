namespace Catalog.Application.Categories.Commands.CreateCategoryCommand;

public class CreateCategoryValidation : AbstractValidator<CreateCategory>
{
    public CreateCategoryValidation()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Kateqoriya adı boş ola bilməz")
            .MinimumLength(3)
            .WithMessage("Kateqoriya adı ən azı 3 simvoldan ibarət olmalıdır")
            .MaximumLength(80)
            .WithMessage("Kateqoriya adı 80 simvoldan artıq ola bilməz");
    }
}
