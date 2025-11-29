using Organization.Application.Branches.Queries.GetBranchesQuery;
using Organization.Application.Companies.Commands.CreateCompanyCommand;
using Organization.Application.Companies.Commands.DeleteCompanyCommand;
using Organization.Application.Companies.Commands.EditCompanyCommand;
using Organization.Application.Companies.Queries.GetCompaniesQuery;
using Organization.Application.Companies.Queries.GetCompanyQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Catalog.Application.Categories.Queries.GetCategoriesQuery;
using Catalog.Application.Products.Queries.GetProductsQuery;
using Ardalis.GuardClauses;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using Organization.Application.Common.Interfaces;

namespace Organization.WebAPI.Controllers;

public class BaseController : ControllerBase
{
    private IMediator _sender;
    protected IMediator Sender => _sender ??= HttpContext.RequestServices.GetService<IMediator>();
}

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : BaseController
{

    [HttpPost]
    [Authorize(Policy = "WriteCompany")]
    public async Task<IActionResult> Create(CreateCompany command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpGet]
    //[Authorize(Policy = "ReadCompany")]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        var result = await Sender.Send(new GetCompanies());
        return Ok(result);
    }

    [HttpGet("{id}/details")]
    //[Authorize(Policy = "ReadCompany")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await Sender.Send(new GetCompany(id));
        return Ok(result);
    }

    [HttpGet("{id}/branches")]
    //[Authorize(Policy = "ReadCompany")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBranchesByCompanyId(string id)
    {
        var result = await Sender.Send(new GetBranchesofCompany(id));
        return Ok(result);
    }

    [HttpGet("{id}/categories")]
    //[Authorize(Policy = "ReadCompany")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategoriesByCompanyId(string id)
    {
        var result = await Sender.Send(new GetCategoriesofCompany(id));
        //result = result.Where(x => x.CompanyId == id).ToList();
        return Ok(result);
    }

    /*[HttpGet("{id}/products")]
    [Authorize(Policy = "ReadCompany")]
    public async Task<IActionResult> GetProductsByCompanyId(string id)
    {
        var result = await Sender.Send(new GetProducts());
        result = result.Where(x => x.CompanyId == id).ToList();
        return Ok(result);
    }*/

    [HttpPut]
    [Authorize(Policy = "WriteCompany")]
    public async Task<IActionResult> Edit(EditCompany command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpPost("edit-image")]
    [Authorize(Policy = "WriteCompany")]
    public async Task<IActionResult> EditImage(string companyId, IFormFile image, CancellationToken cancellationToken)
    {
        Guard.Against.Null(image, nameof(image));
        Guard.Against.LengthOutOfRange(nameof(image), 1, int.MaxValue);

        string _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CompanyLogo");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(image.FileName);

        if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
        {
            return BadRequest("Invalid file extension");
        }

        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
        var path = Path.Combine(_uploadFolderPath, fileName);

        try
        {
            Directory.CreateDirectory(_uploadFolderPath);
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        string imageUrl = $"{Request.Scheme}://{Request.Host}/CompanyLogo/{fileName}";
        var company = await Sender.Send(new GetCompany(companyId));
        await Sender.Send(new EditCompany(company.Id, company.Name, company.Description, imageUrl));
        return Ok(new { Url = imageUrl });
    }

    [HttpDelete]
    [Authorize(Policy = "WriteCompany")]
    public async Task<IActionResult> Delete(DeleteCompany command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }
}
