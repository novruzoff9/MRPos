namespace IdentityServer.DTOs.Employee;

public record EmployeeReturnDto(string Id, string Name, string Email, string Role, string Branch);

public record EmployeeCreateDto(string FirstName, string LastName, string PhoneNumber, string Email, string RoleId, string? CompanyId, string? BranchId);

public record EmployeeFilterDto(string? BranchId = null, string? RoleId = null);
