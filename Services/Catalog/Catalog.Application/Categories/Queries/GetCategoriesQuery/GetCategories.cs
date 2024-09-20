using Catalog.Application.Categories.Queries.GetCategoryQuery;
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Categories.Queries.GetCategoriesQuery;

public record GetCategories : IRequest<List<Category>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategories, List<Category>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> Handle(GetCategories request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.ToListAsync(cancellationToken);
        return categories;
    }
}