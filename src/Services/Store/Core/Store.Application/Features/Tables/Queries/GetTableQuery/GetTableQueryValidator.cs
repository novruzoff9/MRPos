namespace Store.Application.Features.Tables;

public class GetTableQueryValidator : AbstractValidator<GetTableQuery>
{
    public GetTableQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Table ID is required.");
    }
}