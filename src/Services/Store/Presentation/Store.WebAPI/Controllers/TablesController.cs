using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.ResultTypes;
using Store.Application.Common.Models.Table;
using Store.Application.Features.Tables;

namespace Store.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TablesController(
    ISender sender
    ) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTableById(string id)
    {
        var table = await sender.Send(new GetTableQuery(id));
        var response = Response<TableReturnDto>.Success(table, 200);
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllTables()
    {
        var tables = await sender.Send(new GetTablesQuery());
        var response = Response<ICollection<TableReturnDto>>.Success(tables, 200);
        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> CreateTable(CreateTableCommand command)
    {
        var result = await sender.Send(command);
        var response = Response<bool>.Success(result, 201);
        return Ok(response);
    }
    [HttpPatch("{tableId}/deposit")]
    public async Task<IActionResult> UpdateDeposit(string tableId, [FromBody] decimal deposit)
    {
        var result = await sender.Send(new UpdateDepositCommand(tableId, deposit));
        var response = Response<bool>.Success(result, 200);
        return Ok(response);
    }
}