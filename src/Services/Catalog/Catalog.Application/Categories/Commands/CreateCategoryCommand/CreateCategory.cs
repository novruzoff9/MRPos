using Amazon.Runtime;
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Shared.ResultTypes;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Categories.Commands.CreateCategoryCommand;

public record CreateCategory(string Name) : IRequest<Response<NoContent>>;

public class CreateCategoryCommandHandler(IApplicationDbContext context, IIdentityService identityService) : IRequestHandler<CreateCategory, Response<NoContent>>
{
    public async Task<Response<NoContent>> Handle(CreateCategory request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(CreateCategory));


        var category = new Category
        {
            CompanyId = identityService.GetCompanyId,
            Name = request.Name
        };

        await context.Categories.AddAsync(category, cancellationToken);

        var success = await context.SaveChangesAsync(cancellationToken) > 0;

        return Response<NoContent>.Success(200);
    }
}