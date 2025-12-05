using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Context.Seed;

public static class IdentityDbSeeder
{
    public static async Task SeedAsync(IdentityDbContext context)
    {
        if (await context.Roles.AnyAsync() || await context.Users.AnyAsync())
            return;

        var adminRole = new IdentityRole("Admin");
        var bossRole = new IdentityRole("Boss");
        var managerRole = new IdentityRole("Manager");

        var adminUser = new IdentityUser(
            firstName: "admin",
            lastName: "Admin",
            email: "admin@mrpos.com",
            phoneNumber: "+99455-999-49-49",
            password: "Admin123!",
            companyId: "MrPos"
        );

        var adminUserRole = new UserRole(adminUser.Id, adminRole.Id);

        await context.Roles.AddRangeAsync(adminRole, bossRole, managerRole);
        await context.Users.AddAsync(adminUser);
        await context.UserRoles.AddAsync(adminUserRole);

        await context.SaveChangesAsync();
    }
}