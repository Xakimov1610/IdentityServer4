using System.Text.Json;
using identity.Entity;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class Seed
{
    public static async Task InitializeTestUsers(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<User>>();

        var config = scope.ServiceProvider
            .GetRequiredService<IConfiguration>();

        if(!await userManager.Users.AnyAsync())
        {
            return;
        }

        var users = config.GetSection("Ilmhub:IdentityServer:TestUsers").Get<List<identity.Options.User>>();
        foreach(var user in users)
        {
            await userManager.CreateAsync(new User() { Email = user.Email}, user.Password);
        }
    }
    
    public static async Task InitializeConfiguration(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();
        
        var configContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
        configContext.Database.Migrate();

        // var clien = config.GetSection("Ilmhub:IdentityServer:Clients").Get<List<Client>>();
        // System.Console.WriteLine($"{JsonSerializer.Serialize(clien)}");

        if(!await configContext.Clients.AnyAsync())
        {
            var clients = config.GetSection("Ilmhub:IdentityServer:Clients").Get<List<Client>>();
            if(clients != null && clients.Count() > 0)
            {
                var clientEntities = clients.Select(c => c.ToEntity());
                await configContext.Clients.AddRangeAsync(clientEntities);
            }
        }

        if(!await configContext.ApiResources.AnyAsync())
        {
            var apiResources = config.GetSection("Ilmhub:IdentityServer:ApiResources").Get<List<ApiResource>>();
            if(apiResources != null && apiResources.Count() > 0)
            {
                var apiEntities = apiResources.Select(c => c.ToEntity());
                await configContext.ApiResources.AddRangeAsync(apiEntities);
            }
        }

        if(!await configContext.ApiResources.AnyAsync())
        {
            var apiScopes = config.GetSection("Ilmhub:IdentityServer:ApiScopes").Get<List<ApiScope>>();
            if(apiScopes != null && apiScopes.Count() > 0)
            {
                var apiScopeEntities = apiScopes.Select(c => c.ToEntity());
                await configContext.ApiScopes.AddRangeAsync(apiScopeEntities);
            }
        }

        if(!await configContext.IdentityResources.AnyAsync())
        {
            var identityResources = config.GetSection("Ilmhub:IdentityServer:IdentityResources").Get<List<IdentityResource>>();
            if(identityResources != null && identityResources.Count() > 0)
            {
                var identityEntities = identityResources.Select(c => c.ToEntity());
                await configContext.IdentityResources.AddRangeAsync(identityEntities);
            }
        }

        await configContext.SaveChangesAsync();
    }

}