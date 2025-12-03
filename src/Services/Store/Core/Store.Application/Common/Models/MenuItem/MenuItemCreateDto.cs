namespace Store.Application.Common.Models.MenuItem;

public record MenuItemReturnDto(string Id, string Name, string Price, string CategoryName, string Description);
public record MenuItemCreateDto(string BranchId, string CompanyId, string ProductId);