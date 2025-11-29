namespace Catalog.Application.Common.Mapping;

public static class MappingExtension 
{
    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class 
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();
}
