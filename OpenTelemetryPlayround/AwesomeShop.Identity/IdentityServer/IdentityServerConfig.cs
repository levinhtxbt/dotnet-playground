using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace AwesomeShop.Identity.IdentityServer
{
    public class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("api.awesome.shop", "Awesome Shop API")
            };

        public static IEnumerable<Client> Clients(Dictionary<string, string> clientUrls) =>
            new[]
            {
                new Client
                {
                    ClientId = "ro.client",
                    ClientName = "Resource Owner Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "api.awesome.shop" }
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,
                    AllowOfflineAccess = true,

                    RedirectUris = { $"{clientUrls["Mvc"]}/signin-oidc" },
                    PostLogoutRedirectUris = { $"{clientUrls["Mvc"]}/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api.awesome.shop"
                    }
                 },

                new Client
                {
                    ClientId = "swagger",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,

                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris =           { $"{clientUrls["Swagger"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientUrls["Swagger"]}/swagger/oauth2-redirect.html" },
                    AllowedCorsOrigins =     { $"{clientUrls["Swagger"]}" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.awesome.shop"
                    }
                },
                new Client
                {
                    ClientName = "angular_code_client",
                    ClientId = "angular_code_client",
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,

                    RequireClientSecret = false,
                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris = new List<string>
                    {
                        $"{clientUrls["Angular"]}/authentication/login-callback",
                        $"{clientUrls["Angular"]}/silent-renew.html",
                        $"{clientUrls["Angular"]}"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{clientUrls["Angular"]}/unauthorized",
                        $"{clientUrls["Angular"]}/authentication/logout-callback",
                        $"{clientUrls["Angular"]}"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        $"{clientUrls["Angular"]}"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.awesome.shop"
                    }
                },

            };
    }
}
