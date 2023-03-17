using Application.interfaces.Services;
using Identity.Data;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace API_Authentication.IoC;

public static class NativeInjectorConfig
{
    public static void RegisterServices(this WebApplicationBuilder app, IConfiguration configuration)
    {
        app.Services.AddDbContext<IdentityDataContext>(options => 
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
        );

        app.Services.AddScoped<IIdentityService, IdentityService>();
        app.Services.AddDefaultIdentity<IdentityUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<IdentityDataContext>()
                    .AddDefaultTokenProviders();

    }
}