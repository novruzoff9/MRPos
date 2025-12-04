using Store.Application.Common.Models.Company;
using Store.Application.Features.Companies;

namespace Store.Application.Common.Mappings;

internal class CompanyMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Create → Entity
        config.NewConfig<CreateCompanyCommand, Company>()
              .MapToConstructor(true);

        // Entity → DTO (init-only-friendly)
        config.NewConfig<Company, CompanyReturnDto>()
            .ConstructUsing(src => new CompanyReturnDto(
                src.Id,
                src.Name,
                src.PhoneNumber,
                src.Branches != null
                    ? src.Branches.Select(b => new BranchInCompanyDto(
                        b.Id,
                        b.Name,
                      b.PhoneNumber
                    )).ToList()
                    : new List<BranchInCompanyDto>()
            ));

        // Branch → Branch DTO (optional)
        config.NewConfig<Branch, BranchInCompanyDto>();
    }
}
