namespace Store.Application.Common.Models.Product;

public record ProductReturnDto(string Id, string Name, string Description, string Status, string Category, decimal Price);