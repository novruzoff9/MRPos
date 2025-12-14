namespace Store.Application.Common.Models.Table;

public record TableReturnDto(string Id, string Name, int? Capacity, string Status, decimal? Deposit);