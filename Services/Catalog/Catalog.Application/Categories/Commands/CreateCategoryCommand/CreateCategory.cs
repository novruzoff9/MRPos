using Amazon.Runtime;
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Shared.ResultTypes;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Categories.Commands.CreateCategoryCommand;

public record CreateCategory(string Name) : IRequest<Response<NoContent>>;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategory, Response<NoContent>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public CreateCategoryCommandHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Response<NoContent>> Handle(CreateCategory request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(CreateCategory));


        var category = new Category
        {
            CompanyId = _identityService.GetCompanyId,
            Name = request.Name
        };

        await _context.Categories.AddAsync(category, cancellationToken);

        var success = await _context.SaveChangesAsync(cancellationToken) > 0;

        return Response<NoContent>.Success(200);
    }
}