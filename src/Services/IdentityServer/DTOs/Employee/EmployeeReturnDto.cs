namespace IdentityServer.DTOs.Employee;

public record EmployeeReturnDto(string Id, string Name, string Email, string Role, string Branch);

public record EmployeeCreateDto(string FirstName, string LastName, string Email, string Password, string RoleId, string BranchId);
