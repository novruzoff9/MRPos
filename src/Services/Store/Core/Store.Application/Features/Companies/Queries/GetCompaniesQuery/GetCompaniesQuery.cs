using Store.Application.Common.Models.Company;

namespace Store.Application.Features.Companies;

public record GetCompaniesQuery : IRequest<List<CompanyReturnDto>>;

internal class GetCompaniesQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext
    ) : IRequestHandler<GetCompaniesQuery, List<CompanyReturnDto>>
{
    public async Task<List<CompanyReturnDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await dbContext.Companies
            .Include(x => x.Branches)
            .ProjectToType<CompanyReturnDto>(mapper.Config)
            .ToListAsync(cancellationToken);

        return companies;
    }
}