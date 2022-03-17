// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityMembersipAuthServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
        {
            new()
            {
                Name = "resource_api1",
                DisplayName = "Api #1",
                ApiSecrets = { new("api1.secret".Sha256()) },
                Scopes = { "api1.read", "api1.upsert", "api1.delete" }
            },
            new()
            {
                Name = "resource_api2",
                DisplayName = "Api #2",
                ApiSecrets = { new("api2.secret".Sha256()) },
                Scopes = { "api2.read", "api2.upsert", "api2.delete" }
            },
            new(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new("api1.read", "Read permission for API #1"),
            new("api1.upsert", "Upsert permission for API #1"),
            new("api1.delete", "Delete permission for API #1"),
            new("api2.read", "Read permission for API #2"),
            new("api2.upsert", "Upsert permission for API #2"),
            new("api2.delete", "Delete permission for API #2"),
            new(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<Client> Clients => new List<Client>
        {
            new()
            {
                ClientId = "client-app",
                ClientSecrets = { new("client-app.secret".Sha256()) },
                ClientName = "Client Application (Client Credentials)",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "api1.read", "api1.upsert", "api1.delete", "api2.read", "api2.upsert", "api2.delete" }
            },
            new()
            {
                ClientId = "app",
                ClientSecrets = { new("app.secret".Sha256()) },
                ClientName = "Client Application (Hybrid)",
                AllowedGrantTypes = GrantTypes.Hybrid,
                RequirePkce = false,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "api1.read",
                    "api1.upsert",
                    "api1.delete",
                    "api2.read",
                    "api2.upsert",
                    "api2.delete",
                    "CountryAndCity",
                    "Roles"
                },
                RedirectUris = { "https://localhost:7003/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:7003/signout-callback-oidc" },
                AccessTokenLifetime = 3600,
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = 30,
                RequireConsent = true
            },
            new()
            {
                ClientId = "angular-app",
                ClientName = "Angular App (JavaScript Client)",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1.read",
                    "CountryAndCity",
                    "Roles"
                },
                AccessTokenLifetime = 60,
                AllowedCorsOrigins = { "http://localhost:4200" },
                RedirectUris =
                {
                    "http://localhost:4200/signin-callback",
                    "http://localhost:4200/silent-callback"
                },
                PostLogoutRedirectUris = { "http://localhost:4200" }
            },
            new()
            {
                ClientId = "pwd-app",
                ClientSecrets = { new("pwd-app.secret".Sha256()) },
                ClientName = "Client Application (Resource Owner Password)",
                //AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.Email,
                    "api1.read",
                    "api1.upsert",
                    "api1.delete",
                    "api2.read",
                    "api2.upsert",
                    "api2.delete",
                    "CountryAndCity",
                    "Roles",
                    IdentityServerConstants.LocalApi.ScopeName
                },
                AccessTokenLifetime = 3600,
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = 30,
            },
        };

        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(), // sub
            new IdentityResources.Profile(), // name, family_name, given_name, middle_name, nickname, preferred_username, profile, picture, website, gender, birthdate, zoneinfo, locale, updated_at
            new IdentityResource
            {
                Name = "CountryAndCity",
                DisplayName = "Country and City",
                Description = "User's country and city information",
                UserClaims = { "country", "city" }
            },
            new IdentityResource
            {
                Name = "Roles",
                DisplayName = "Roles",
                Description = "User's roles",
                UserClaims = { "role" }

            }
        };
    }
}