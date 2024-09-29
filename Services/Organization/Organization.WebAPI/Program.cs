using MediatR;
using Microsoft.EntityFrameworkCore;
using Organization.Application;
using Organization.Application.Common.Behaviors;
using Organization.Application.Common.Interfaces;
using Organization.Infrastructure;
using Organization.WebAPI;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Infrastructure Layer
builder.Services.AddInfrastructureServices(builder.Configuration);

//Application Layer
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddWebApiServices(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
