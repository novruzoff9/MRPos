namespace Order.Domain.Common;
public interface ISoftDeletable
{
    DateTime? DeletedAt { get; }
    bool IsDeleted { get; }
    void SoftDelete();
}
