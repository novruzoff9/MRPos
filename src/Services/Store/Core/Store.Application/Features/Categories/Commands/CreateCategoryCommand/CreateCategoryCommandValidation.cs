namespace Store.Application.Features.Categories;

public class CreateCategoryCommandValidation : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidation()
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
