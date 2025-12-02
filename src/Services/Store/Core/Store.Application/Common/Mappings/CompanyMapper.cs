
using Store.Application.Features.Companies;

namespace Store.Application.Common.Mappings;

public class CompanyMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCompanyCommand, Company>()
              .MapToConstructor(true);
    }
}
