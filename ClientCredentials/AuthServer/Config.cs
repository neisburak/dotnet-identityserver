using IdentityServer4.Models;

namespace AuthServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
        {
            new()
            {
                Name = "resource_api1",
                ApiSecrets = { new("api1.secret".Sha256()) },
                Scopes = { "api1.read", "api1.upsert", "api1.delete" }
            },
            new()
            {
                Name = "resource_api2",
                ApiSecrets = { new("api2.secret".Sha256()) },
                Scopes = { "api2.read", "api2.upsert", "api2.delete" }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new("api1.read", "Read permission for API #1"),
            new("api1.upsert", "Upsert permission for API #1"),
            new("api1.delete", "Delete permission for API #1"),
            new("api2.read", "Read permission for API #2"),
            new("api2.upsert", "Upsert permission for API #2"),
            new("api2.delete", "Delete permission for API #2")
        };

        public static IEnumerable<Client> Clients => new List<Client>
        {
            new()
            {
                ClientId = "app",
                ClientSecrets = { new("app.secret".Sha256()) },
                ClientName = "Client #1 Application",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "api1.read" }
            },
            new()
            {
                ClientId = "other-app",
                ClientSecrets = { new("other-app.secret".Sha256()) },
                ClientName = "Client #2 Application",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "api1.read", "api1.upsert", "api1.delete", "api2.read", "api2.upsert", "api2.delete" }
            }
        };
    }
}