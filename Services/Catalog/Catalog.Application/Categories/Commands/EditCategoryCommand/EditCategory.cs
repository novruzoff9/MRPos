using Catalog.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Categories.Commands.EditCategoryCommand;

public record EditCategory(int Id, string Name) : IRequest<bool>;

public class EditCategoryCommandHandler : IRequestHandler<EditCategory, bool>
{
    private readonly IApplicationDbContext _context;

    public EditCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(EditCategory request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(request.Id, nameof(request));
        Guard.Against.NotFound(request.Name, nameof(request));

        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (category == null) { return false; }

        category.Name = request.Name;

        _context.Categories.Update(category);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
