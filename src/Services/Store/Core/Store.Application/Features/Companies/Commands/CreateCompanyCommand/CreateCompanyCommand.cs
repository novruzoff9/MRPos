namespace Store.Application.Features.Companies;

public record CreateCompanyCommand (
    string Name,
    string PhoneNumber,
    string? Description,
    string? LogoUrl) : IRequest<string>;

internal class CreateCompanyCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext
    ) : IRequestHandler<CreateCompanyCommand, string>
{
    public async Task<string> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = mapper.Map<Company>(request);
        dbContext.Companies.Add(company);
        await dbContext.SaveChangesAsync(cancellationToken);

        return company.Id;
    }
}
