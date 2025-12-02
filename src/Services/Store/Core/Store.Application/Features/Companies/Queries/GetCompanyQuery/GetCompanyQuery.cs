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
            .ProjectToType<CompanyReturnDto>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        // TODO: Exception
        return company;
    }
}