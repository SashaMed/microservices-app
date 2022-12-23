using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Identity
{
    public static class StaticData
    {
        public const string Admin = "Admin";

        public const string Customer = "Customer";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>()
        {
            new ApiScope("mango", "Mango Server"),
            new ApiScope("read", "read data"),
            new ApiScope("write", "write data"),
            new ApiScope("delete", "delete data"),
        };

        public static IEnumerable<Client> Clients => new List<Client>()
        {
            new Client
            {
                ClientId = "client",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "read", "write", "profile" }
            },
            new Client
            {
                ClientId = "mango",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "https://localhost:44311/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:44311/signout-callback-oidc"  },
                AllowedScopes = 
                { 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "mango",
                }
            },
        };
    }
}
