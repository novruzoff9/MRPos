namespace Catalog.Application.Companies.Commands.EditCompanyCommand;

public record EditCompany(string Id, string Name, string Description, string LogoUrl) : IRequest<bool>;

public class EditCompanyCommandHandler : IRequestHandler<EditCompany, bool>
{
    private readonly IApplicationDbContext _context;

    public EditCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(EditCompany request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(request.Id, nameof(request));
        Guard.Against.NotFound(request.Name, nameof(request));
        Guard.Against.NotFound(request.Description, nameof(request));

        var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (company == null) { return false; }

        company.Name = request.Name;
        company.Description = request.Description;
        company.LogoUrl = request.LogoUrl;
        company.LastModified = DateTime.UtcNow;

        _context.Companies.Update(company);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
