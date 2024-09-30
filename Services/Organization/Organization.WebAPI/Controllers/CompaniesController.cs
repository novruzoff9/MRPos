using Catalog.Application.Branches.Queries.GetBranchesQuery;
using Catalog.Application.Companies.Commands.CreateCompanyCommand;
using Catalog.Application.Companies.Commands.DeleteCompanyCommand;
using Catalog.Application.Companies.Commands.EditCompanyCommand;
using Catalog.Application.Companies.Queries.GetCompaniesQuery;
using Catalog.Application.Companies.Queries.GetCompanyQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    [Authorize(Policy = "ReadCompany")]
    public async Task<IActionResult> Get()
    {
        var result = await Sender.Send(new GetCompanies());
        return Ok(result);
    }

    [HttpGet("{id}/details")]
    [Authorize(Policy = "ReadCompany")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await Sender.Send(new GetCompany(id));
        return Ok(result);
    }

    [HttpGet("{id}/branches")]
    [Authorize(Policy = "ReadCompany")]
    public async Task<IActionResult> GetBranchesByCompanyId(string id)
    {
        var result = await Sender.Send(new GetBranches());
        result = result.Where(x => x.CompanyId == id).ToList();
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Policy = "WriteCompany")]
    public async Task<IActionResult> Edit(EditCompany command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Policy = "WriteCompany")]
    public async Task<IActionResult> Delete(DeleteCompany command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }
}
