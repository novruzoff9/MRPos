using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Tables.Commands.CreateTableCommand;
using Organization.Application.Tables.Commands.DeleteTableCommand;
using Organization.Application.Tables.Commands.EditTableCommand;
using Organization.Application.Tables.Queries.GetTableQuery;
using Organization.Application.Tables.Queries.GetTablesQuery;

namespace Organization.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class TablesController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await Sender.Send(new GetTables());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(GetTable query)
    {
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTable command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Edit(EditTable command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteTable command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }
}
