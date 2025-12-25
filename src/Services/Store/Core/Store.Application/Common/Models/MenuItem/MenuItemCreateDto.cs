namespace Store.Application.Common.Models.MenuItem;

public record MenuItemReturnDto(string Id, string ProductId, string Name, string Price, string CategoryName, string Description);

public record MenuItemCreateDto(string BranchId, string CompanyId, string ProductId);

public record MenuItemDto(string ProductId);