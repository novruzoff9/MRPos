using Catalog.Application.Companies.Commands.CreateCompanyCommand;
using Catalog.Application.Companies.Queries.GetCompaniesQuery;
using MediatR;
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
    public async Task<IActionResult> Create(CreateCompany command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await Sender.Send(new GetCompanies());
        return Ok(result);
    }
}
