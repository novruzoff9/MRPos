using Catalog.Application;
using Catalog.Infrasturucture;
using Microsoft.AspNetCore.Authorization;
using Shared.Extensions;
using Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Infrastructure services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Application services
builder.Services.AddApplicationServices(builder.Configuration);


var reqriedAuthorizationPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddControllers(options =>
{
    //options.Filters.Add(new AuthorizeFilter(requiredAuthorizationPolicy));
});
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureAuth(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RestrictAccessMiddleware>();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
