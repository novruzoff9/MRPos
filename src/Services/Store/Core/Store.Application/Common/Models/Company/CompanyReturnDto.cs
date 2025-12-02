namespace Store.Application.Common.Models.Company;

public record CompanyReturnDto(string Id, string Name, string PhoneNumber, List<BranchInCompanyDto> Branches);

public record BranchInCompanyDto(string Id, string Name, string PhoneNumber);