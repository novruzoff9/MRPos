using Store.Application.Common.Models.Company;

namespace Store.Application.Features.Companies;

public record GetCompanyQuery(string Id) : IRequest<CompanyReturnDto>;

public class GetCompanyQueryHandler(
    IApplicationDbContext dbContext
    ) : IRequestHandler<GetCompanyQuery, CompanyReturnDto>
{
    public async Task<CompanyReturnDto> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {
        var company = await dbContext.Companies
            .Include(c => c.Branches)
            .Where(c => c.Id == request.Id)
            .ProjectToType<CompanyReturnDto>()
            .FirstOrDefaultAsync(cancellationToken);
        if (company is null)
            throw new NotFoundException($"Company not found with ID: {request.Id}");
        return company;
    }
}
