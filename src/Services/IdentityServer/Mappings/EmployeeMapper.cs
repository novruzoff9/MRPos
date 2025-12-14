using IdentityServer.DTOs.Employee;
using IdentityServer.Models;
using Mapster;

namespace IdentityServer.Mappings;

public class EmployeeMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<EmployeeCreateDto, IdentityUser>()
            .ConstructUsing(dto => new IdentityUser(
                firstName: dto.FirstName,
                lastName: dto.LastName,
                email: dto.Email,
                phoneNumber: dto.PhoneNumber,
                password: "Pass123!",
                companyId: dto.CompanyId,
                branchId: dto.BranchId
            ));
    }
}
