namespace Store.Application.Features.Companies;

public record DeleteCompanyCommand(string Id) : IRequest<bool>;

public class DeleteCompanyCommandHandler(
    IApplicationDbContext dbContext
    ) : IRequestHandler<DeleteCompanyCommand, bool>
{
    public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (company == null)
            throw new Exception("Company not Found");
        company.SoftDelete();
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}