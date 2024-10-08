using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Organization.Application;
using Organization.WebAPI;
using Shared.Services;

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

//WebApi Layer
builder.Services.AddWebApiServices(builder.Configuration);

//Authorization
builder.Services.AddHttpContextAccessor();
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("roles");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServerUrl"];
        options.Audience = "OrganizationAPIFullAccess";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            RoleClaimType = "roles",
            NameClaimType = "sub"
        };
    });

builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("WriteCompany", policy => policy.RequireRole("superadmin"));
    options.AddPolicy("ReadCompany", policy => policy.RequireRole("superadmin", "admin", "director"));

    options.AddPolicy("WriteBranch", policy => policy.RequireRole("superadmin", "admin", "director"));
    options.AddPolicy("ReadBranch", policy => policy.RequireRole("superadmin", "admin", "director", "branchdirector"));
});

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

app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
