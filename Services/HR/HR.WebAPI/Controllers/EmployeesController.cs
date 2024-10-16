﻿using HR.WebAPI.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.ResultTypes;
using Shared.Services;
using System.Text;

namespace HR.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly ISharedIdentityService _identityService;
    private readonly IHttpClientFactory _httpClientFactory;

    public EmployeesController(ISharedIdentityService identityService, IHttpClientFactory httpClientFactory)
    {
        _identityService = identityService;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var client = _httpClientFactory.CreateClient("employees");
        var response = await client.GetAsync($"http://localhost:5001/api/Employees?companyId={_identityService.GetCompanyId}");
        var employees = await response.Content.ReadFromJsonAsync<List<UserDto>>();

        var result = Response<List<UserDto>>.Success(employees, 200);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateUserDto request)
    {
        var client = _httpClientFactory.CreateClient("employees");
        var jsondata = JsonConvert.SerializeObject(request);
        StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"http://localhost:5001/api/Employees?companyId={_identityService.GetCompanyId}", stringContent);

        Response<NoContent> result;
        if (response.IsSuccessStatusCode)
        {
            result = Response<NoContent>.Success(200);
        }
        else
        {
            result = Response<NoContent>.Fail("İşçini artırmaq mümkün olmadı", 400);
        }
        return Ok(result);
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRoleToUser(string userId, string roleId)
    {
        var client = _httpClientFactory.CreateClient("employees");
        var response = await client.PostAsync($"http://localhost:5001/api/roles/assign-role?userId={userId}&roleId={roleId}", null);

        Response<NoContent> result;

        if (response.IsSuccessStatusCode)
        {
            result = Response<NoContent>.Success(200);
        }
        else
        {
            result = Response<NoContent>.Fail("Error oldu",400);
        }
        return Ok(result);
    }
}