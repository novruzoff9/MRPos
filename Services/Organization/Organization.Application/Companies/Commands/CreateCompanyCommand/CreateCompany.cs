using Organization.Application.Common.Services;

namespace Catalog.Application.Companies.Commands.CreateCompanyCommand;

public record CreateCompany(string Name, string Description, string LogoUrl) : IRequest<bool>;

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompany, bool>
{
    private readonly IApplicationDbContext _context;

    public CreateCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateCompany request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(CreateCompany));
        GenerateOrganizationId IdGenerator = new GenerateOrganizationId(_context);
        
        var company = new Company
        {
            Id = await IdGenerator.GenerateUniqueCompanyIdAsync(),
            Name = request.Name,
            Description = request.Description,
            LogoUrl = request.LogoUrl,
            Created = DateTime.UtcNow
        };

        await _context.Companies.AddAsync(company, cancellationToken);

        var success = await _context.SaveChangesAsync(cancellationToken) > 0;

        return success;
    }
}

