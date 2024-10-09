using Organization.Application.Common.Services;
using Shared.Services;

namespace Organization.Application.Companies.Commands.CreateCompanyCommand;

public record CreateCompany(string Name, string Description, string LogoUrl) : IRequest<bool>;

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompany, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public CreateCompanyCommandHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
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
            Created = DateTime.UtcNow,
            CreatedBy = _identityService.GetUserId
        };

        await _context.Companies.AddAsync(company, cancellationToken);

        var success = await _context.SaveChangesAsync(cancellationToken) > 0;

        return success;
    }
}

