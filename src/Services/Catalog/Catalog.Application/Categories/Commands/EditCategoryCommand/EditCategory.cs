using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.ResultTypes;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Categories.Commands.EditCategoryCommand;

public record EditCategory(int Id, string Name) : IRequest<Response<Category>>;

public class EditCategoryCommandHandler : IRequestHandler<EditCategory, Response<Category>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public EditCategoryCommandHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Response<Category>> Handle(EditCategory request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(request.Id, nameof(request));
        Guard.Against.NotFound(request.Name, nameof(request));

        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == _identityService.GetCompanyId, cancellationToken);
        if (category == null) { return Response<Category>.Fail("Daxil etdiyiniz Kateqoriya dəyəri yanlışdır", 400); }

        category.Name = request.Name;

        _context.Categories.Update(category);

        await _context.SaveChangesAsync(cancellationToken);
        return Response<Category>.Success(category, 200);
    }
}
