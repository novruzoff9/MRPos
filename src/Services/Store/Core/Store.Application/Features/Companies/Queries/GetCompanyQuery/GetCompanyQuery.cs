using Store.Application.Common.Models.Company;
using Store.Domain.Entities;

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
        if (company is null)
            throw new NotFoundException($"Company not found with ID: {request.Id}");
        return company;
    }
}