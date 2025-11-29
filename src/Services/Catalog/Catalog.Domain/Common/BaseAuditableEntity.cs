namespace Catalog.Domain.Common;

public class BaseAuditableEntity : BaseEntity
{
    [BsonSerializer(typeof(DateTimeOffsetSerializer))]
    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    [BsonSerializer(typeof(DateTimeOffsetSerializer))]
    public DateTimeOffset Modified { get; set; }

    public string? LastModifiedBy { get; set; }
}
