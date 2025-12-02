namespace Store.Domain.Common;

public abstract class BaseEntity
{
    public string Id { get; private set; }
    protected BaseEntity()
    {
        Id = Guid.NewGuid().ToString();
    }
}
