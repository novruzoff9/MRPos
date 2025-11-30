using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using WebApiGateway.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Environment-specific Ocelot configuration
builder.Configuration.AddJsonFile($"Configurations/ocelot.json", optional: false, reloadOnChange: true);

// Ocelot services
builder.Services.AddOcelot(builder.Configuration);

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("GatewayCors", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Use CORS policy
app.UseCors("GatewayCors");

// Custom Correlation ID middleware
app.UseMiddleware<CorrelationIdMiddleware>();

await app.UseOcelot();

app.Run();
