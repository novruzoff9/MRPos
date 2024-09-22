using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Categories.Commands.CreateCategoryCommand;

public record CreateCategory(string Name) : IRequest<bool>;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategory, bool>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateCategory request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(CreateCategory));

        var category = new Category
        {
            Name = request.Name
        };

        await _context.Categories.AddAsync(category, cancellationToken);

        var success = await _context.SaveChangesAsync(cancellationToken) > 0;

        return success;
    }
}