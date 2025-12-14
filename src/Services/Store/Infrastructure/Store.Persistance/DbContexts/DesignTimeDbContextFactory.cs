using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shared.Interfaces;
using System.Security.Claims;

namespace Store.Persistance.DbContexts;

public class DesignTimeDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new ApplicationDbContext(
            new FakeIdentityService(),
            options
        );
    }

    private class FakeIdentityService : IIdentityService
    {
        public string GetCompanyId => "SYSTEM";
        public string GetRole => string.Empty;
        public string GetUserId => string.Empty;
        public string GetBranchId => string.Empty;
        public ClaimsPrincipal GetUser => new ClaimsPrincipal();
    }
}
