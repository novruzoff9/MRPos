using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.General;
using Shared.ResultTypes;
using Store.Application.Common.Interfaces;
using Store.Application.Common.Models.Branch;
using Store.Application.Features.Branches;
using Store.Domain.Entities;

namespace Store.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BranchesController(
    ISender sender,
    ILookupService lookupService
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBranches()
    {
        var branches = await sender.Send(new GetBranchesQuery());
        var response = Response<ICollection<BranchReturnDto>>.Success(branches, 200);
        return Ok(response);
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> GetBranchesLookup()
    {
        var branchesLookup = await lookupService.GetLookupAsync<Branch, string>();
        var response = Response<List<LookupDto<string>>>.Success(branchesLookup, 200);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBranch(CreateBranchCommand command)
    {
        var result = await sender.Send(command);
        var response = Response<bool>.Success(result, 204);
        return Ok(response);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBranch(string id)
    {
        var branch = await sender.Send(new GetBranchQuery(id));
        var response = Response<BranchReturnDto>.Success(branch, 200);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBranch(string id)
    {
        var result = await sender.Send(new DeleteBranchCommand(id));
        var response = Response<bool>.Success(result, 200);
        return Ok(response);
    }

    [HttpGet("service-fee")]
    public async Task<IActionResult> GetBranchServiceFee()
    {
        var serviceFee = await sender.Send(new GetBranchServiceFeeQuery());
        var response = Response<decimal>.Success(serviceFee, 200);
        return Ok(response);
    }

    [HttpPatch("service-fee")]
    public async Task<IActionResult> UpdateBranchServiceFee(UpdateBranchServiceFeeCommand command)
    {
        var result = await sender.Send(command);
        var response = Response<bool>.Success(result, 200);
        return Ok(response);
    }
}