namespace Store.Domain.Enums;

public enum ProductStatus
{
    None = 0,
    New = 1,
    Popular = 2,
    NewPopular = New | Popular
}