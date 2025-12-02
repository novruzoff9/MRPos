using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.ResultTypes;
using Store.Application.Common.Models.Company;
using Store.Application.Features.Companies;
using Store.Application.Features.Companies.Queries.GetCompaniesQuery;

namespace Store.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController(
    ISender sender
    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyCommand command)
    {
        var companyId = await sender.Send(command);
        var response = Response<string>.Success(companyId, 201);
        return CreatedAtAction(nameof(GetCompanies), new { id = companyId }, response);
    }
    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await sender.Send(new GetCompaniesQuery());
        var response = Response<List<CompanyReturnDto>>.Success(companies, 200);
        return Ok(response);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCompany(string id)
    {
        var company = await sender.Send(new GetCompanyQuery(id));
        var response = Response<CompanyReturnDto>.Success(company, 200);
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteCompanyById(string id)
    {
        var result = await sender.Send(new DeleteCompanyCommand(id));
        var response = Response<bool>.Success(result, 200);
        return Ok(response);
    }
}
