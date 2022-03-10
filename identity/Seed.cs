using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;

public static class Seed
{
    
    public static async Task InitializeDatabase(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();
        
        var configContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
        configContext.Database.Migrate();

        if(!await configContext.Clients.AnyAsync())
        {
            var clients = config.GetValue<List<Client>>(config["Ilmhub:IdentityServer:Clients"]);
            if(clients != null && clients.Count() > 0)
            {
                var clientEntities = clients.Select(c => c.ToEntity());
                await configContext.Clients.AddRangeAsync(clientEntities);
            }
        }

        if(!await configContext.ApiResources.AnyAsync())
        {
            var apiResources = config.GetValue<List<ApiResource>>("Ilmhub:IdentityServer:ApiResources");
            if(apiResources != null && apiResources.Count() > 0)
            {
                var apiEntities = apiResources.Select(c => c.ToEntity());
                await configContext.ApiResources.AddRangeAsync(apiEntities);
            }
        }

        if(!await configContext.ApiResources.AnyAsync())
        {
            var apiScopes = config.GetValue<List<ApiScope>>("Ilmhub:IdentityServer:ApiScopes");
            if(apiScopes != null && apiScopes.Count() > 0)
            {
                var apiScopeEntities = apiScopes.Select(c => c.ToEntity());
                await configContext.ApiScopes.AddRangeAsync(apiScopeEntities);
            }
        }

        if(!await configContext.IdentityResources.AnyAsync())
        {
            var identityResources = config.GetValue<List<IdentityResource>>("Ilmhub:IdentityServer:IdentityResources");
            if(identityResources != null && identityResources.Count() > 0)
            {
                var identityEntities = identityResources.Select(c => c.ToEntity());
                await configContext.IdentityResources.AddRangeAsync(identityEntities);
            }
        }

        // await Task.CompletedTask;
    }

}