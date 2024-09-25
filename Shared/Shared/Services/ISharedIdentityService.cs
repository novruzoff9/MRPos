using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Shared.Services
{
    public interface ISharedIdentityService
    {
        public string GetUserId { get; }
    }

    public class SharedIdentityService : ISharedIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;

        /*
        Using in Service
        Program.cs:
        // Add HttpContextAccessor service to the container
        builder.Services.AddHttpContextAccessor();

        // Remove default inbound claim type mappings for "sub"
        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
        
        // JWT Bearer Authentication
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration["IdentityServerUrl"];
                options.Audience = "BasketAPIFullAccess";
                options.RequireHttpsMetadata = false;
        
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "role",
                    NameClaimType = "sub"
                };
            });
        
        // Dependency Injection for SharedIdentityService
        builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();

         
         */
    }
}
