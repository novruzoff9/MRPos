namespace Order.Domain.Common;

public abstract class BaseAuditableEntity(string? createdBy = null) : BaseEntity()
{
    public DateTimeOffset Created { get; init; } = DateTimeOffset.UtcNow;
    public string? CreatedBy { get; init; } = createdBy;
    public DateTimeOffset LastModified { get; protected set; } = DateTimeOffset.UtcNow;
    public string? LastModifiedBy { get; protected set; } = createdBy;

    public void SetModified(string modifiedBy)
    {
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}
