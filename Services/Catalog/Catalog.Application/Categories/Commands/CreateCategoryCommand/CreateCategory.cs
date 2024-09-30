using Amazon.Runtime;
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Categories.Commands.CreateCategoryCommand;

public record CreateCategory(string Name, string CompanyId) : IRequest<bool>;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategory, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenService _tokenService;

    public CreateCategoryCommandHandler(IApplicationDbContext context, IHttpClientFactory httpClientFactory, ITokenService tokenService)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _tokenService = tokenService;
    }

    public async Task<bool> Handle(CreateCategory request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(CreateCategory));

        var client = _httpClientFactory.CreateClient("company");
        var token = await _tokenService.GetTokenAsync();

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var companyResponse = await client.GetAsync($"http://localhost:5005/api/Companies/{request.CompanyId}/details");

        if (companyResponse.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return false;
        }

        var category = new Category
        {
            CompanyId = request.CompanyId,
            Name = request.Name
        };

        await _context.Categories.AddAsync(category, cancellationToken);

        var success = await _context.SaveChangesAsync(cancellationToken) > 0;

        return success;
    }
}