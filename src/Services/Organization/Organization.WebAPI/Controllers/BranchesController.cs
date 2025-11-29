using Organization.Application.Branches.Commands.DeleteBranchCommand;
using Organization.Application.Branches.Commands.EditBranchCommand;
using Organization.Application.Branches.Queries.GetBranchesQuery;
using Organization.Application.Branches.Queries.GetBranchQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Organization.Application.Branches.Commands.CreateBranchCommand;
using Organization.Application.Common.Models.Branch;
using Shared.ResultTypes;
using Organization.Infrastructure;

namespace Organization.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BranchesController : BaseController
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public BranchesController(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    [HttpGet]
    [Authorize(Policy = "ReadBranch")]
    public async Task<IActionResult> Get()
    {
        var result = await Sender.Send(new GetBranches());
        return Ok(result);
    }

    [HttpGet("summary")]
    [Authorize(Policy = "ReadBranch")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await Sender.Send(new GetBranchesSummary());
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "ReadBranch")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await Sender.Send(new GetBranch(id));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "WriteBranch")]
    public async Task<IActionResult> Create(CreateBranch command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Policy = "WriteBranch")]
    public async Task<IActionResult> Edit(EditBranch command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "WriteBranch")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await Sender.Send(new DeleteBranch(id));
        return Ok(result);
    }

    [HttpGet("branch-serviceFee/{branchId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetServiceFee(string branchId)
    {
        var branch = _context.Branches.FirstOrDefault(x => x.Id == branchId);
        var serviceFee = branch.ServiceFee;
        return Ok(serviceFee);
    }
}
