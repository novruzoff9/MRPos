using Store.Domain.Entities;

namespace Store.Application.Features.Companies;

public record DeleteCompanyCommand(string Id) : IRequest<bool>;

public class DeleteCompanyCommandHandler(
    IApplicationDbContext dbContext
    ) : IRequestHandler<DeleteCompanyCommand, bool>
{
    public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (company is null)
            throw new NotFoundException($"Company not found with ID: {request.Id}");
        company.SoftDelete();
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
