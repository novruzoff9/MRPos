using Shared.Models.General;
using Store.Domain.Common;

namespace Store.Application.Common.Interfaces;

public interface ILookupService
{
    Task<List<LookupDto<TId>>> GetLookupAsync<TEntity, TId>(string name = "Name") where TEntity : BaseEntity;
}
