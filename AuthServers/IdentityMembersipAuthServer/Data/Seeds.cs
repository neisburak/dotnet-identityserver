using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

namespace IdentityMembersipAuthServer.Data
{
    public static class Seeds
    {
        public static async Task Seed(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var item in Config.Clients)
                {
                    context.Clients.Add(item.ToEntity());
                }
            }

            if (!context.ApiResources.Any())
            {
                foreach (var item in Config.ApiResources)
                {
                    context.ApiResources.Add(item.ToEntity());
                }
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var item in Config.ApiScopes)
                {
                    context.ApiScopes.Add(item.ToEntity());
                }
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var item in Config.IdentityResources)
                {
                    context.IdentityResources.Add(item.ToEntity());
                }
            }

            await context.SaveChangesAsync();
        }
    }
}