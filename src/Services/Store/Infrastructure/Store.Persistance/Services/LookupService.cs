using Shared.Models.General;
using Store.Domain.Common;
using Store.Persistance.DbContexts;

namespace Store.Persistance.Services;

public class LookupService(ApplicationDbContext dbContext) : ILookupService
{
    public async Task<List<LookupDto<TId>>> GetLookupAsync<TEntity, TId>(string name = "Name") where TEntity : BaseEntity
    {
        var dbSet = dbContext.Set<TEntity>();

        var idProp = typeof(TEntity).GetProperty("Id");
        var nameProp = typeof(TEntity).GetProperty(name);

        if (idProp == null || nameProp == null)
            throw new Exception($"{typeof(TEntity).Name} entity-də Id və Name property mövcud deyil.");

        return await dbSet
            .Select(e => new LookupDto<TId>(
                (TId)idProp.GetValue(e),
                (string)nameProp.GetValue(e)
            ))
            .ToListAsync();
    }
}
