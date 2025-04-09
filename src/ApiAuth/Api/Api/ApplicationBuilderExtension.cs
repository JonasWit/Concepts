using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Api;

public static class ApplicationBuilderExtension
{
    public static async Task<WebApplication> BuildAndSetup(this WebApplicationBuilder builder)
    {
        var app = builder.Build();
        using var scope = app.Services.CreateScope();
        
        var usrMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var user = new IdentityUser() { UserName = "test@test.com", Email = "test@test.com" };
        
        await usrMgr.CreateAsync(user, password: "123456");
        await usrMgr.AddClaimAsync(user, new Claim("role", "janitor"));

        return app;
    }
}