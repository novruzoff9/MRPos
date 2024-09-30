using Catalog.Application.Branches.Commands.DeleteBranchCommand;
using Catalog.Application.Branches.Commands.EditBranchCommand;
using Catalog.Application.Branches.Queries.GetBranchesQuery;
using Catalog.Application.Branches.Queries.GetBranchQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Branches.Commands.CreateBranchCommand;

namespace Organization.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class BranchesController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await Sender.Send(new GetBranches());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await Sender.Send(new GetBranch(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBranch command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Edit(EditBranch command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteBranch command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }
}
