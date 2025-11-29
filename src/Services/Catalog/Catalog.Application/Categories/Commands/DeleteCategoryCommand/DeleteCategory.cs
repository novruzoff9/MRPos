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

namespace Catalog.Application.Categories.Commands.DeleteCategoryCommand;

public record DeleteCategory(int Id) : IRequest<Response<Category>>;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategory, Response<Category>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public DeleteCategoryCommandHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Response<Category>> Handle(DeleteCategory request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.Id, nameof(request.Id));

        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == _identityService.GetCompanyId, cancellationToken);
        if(category == null) { return Response<Category>.Fail("Daxil etdiyiniz Kateqoriya dəyəri yanlışdır", 400); }
        _context.Categories.Remove(category);

        await _context.SaveChangesAsync(cancellationToken);
        return Response<Category>.Success(category, 200);
    }
}
