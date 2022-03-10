using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

public static class Seed
{
    private static async Task InitializeDatabse(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();
        scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

        var configContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        configContext.Database.Migrate();

        if(!await configContext.Clients.AnyAsync())
        {
            
        }
    }
}