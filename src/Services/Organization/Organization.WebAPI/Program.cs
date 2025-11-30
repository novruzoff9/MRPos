using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Organization.Application;
using Organization.WebAPI;
using Shared.Extensions;
using Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Infrastructure Layer
builder.Services.AddInfrastructureServices(builder.Configuration);

//Application Layer
builder.Services.AddApplicationServices(builder.Configuration);

//WebApi Layer
builder.Services.AddWebApiServices(builder.Configuration);

//Authorization
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureAuth(builder.Configuration);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("WriteCompany", policy => policy.RequireRole("superadmin", "admin"))
    .AddPolicy("ReadCompany", policy => policy.RequireRole("superadmin", "admin", "director"))
    .AddPolicy("WriteBranch", policy => policy.RequireRole("superadmin", "admin", "director"))
    .AddPolicy("ReadBranch", policy => policy.RequireRole("superadmin", "admin","boss", "director", "branchdirector"));

var requiredAuthorizationPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(requiredAuthorizationPolicy));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<RestrictAccessMiddleware>();
app.UseStaticFiles();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
