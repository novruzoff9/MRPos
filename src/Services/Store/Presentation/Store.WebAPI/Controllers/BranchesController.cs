using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.ResultTypes;
using Store.Application.Common.Models.Branch;
using Store.Application.Features.Branches;

namespace Store.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BranchesController(
    ISender sender
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBranches()
    {
        var branches = await sender.Send(new GetBranchesQuery());
        var response = Response<List<BranchReturnDto>>.Success(branches, 200);
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
}
