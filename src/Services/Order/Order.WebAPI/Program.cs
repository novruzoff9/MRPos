using Menu.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Order.WebAPI.Hubs;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});

builder.Services.AddHttpContextAccessor();

//Authorization
builder.Services.AddHttpContextAccessor();
//builder.Services.ConfigureAuth(builder.Configuration);


builder.Services.AddAuthorizationBuilder()
    .AddPolicy("WriteOrder", policy => policy.RequireRole("superadmin", "admin", "director", "waiter"))
    .AddPolicy("ReadOrder", policy => policy.RequireRole("superadmin", "admin", "director"));

var requiredAuthorizationPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<RestrictAccessMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<OrderHub>("/orderhub");

app.Run();
