using Duende.IdentityModel;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Shop.IdentityServer.Configuration
{
    public class IdentityConfiguration
    {
        public const string ADMIN = "Admin";
        public const string CLIENT = "Client";

        public static IEnumerable<IdentityResource> IdentityResources =>  // permite modelar um scope que permite exibir user
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(), // usado para receber o token
            new IdentityResources.Profile(),
            new IdentityResources.Email()
            /*new IdentityResource()
            {
                Name = "verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Email
                }
            }*/
        };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
            new ApiScope("shop", "Shop Server"),
            new ApiScope(name: "read", displayName: "My API read"),
            new ApiScope(name: "write", displayName: "My Api write"),
            new ApiScope(name: "delete", displayName: "delete"),
            };

        public static IEnumerable<Client> Clients => // Clients registered!
            new Client[]
            {
            new Client  // Client Credentials
            {
                ClientId = "client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = { "read", "write", "delete" }
            },
            // interactive ASP.NET Core Web App
            new Client  // Authorization Code
            {
                ClientId = "shop",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code, // via code

                // where to redirect to after login
                RedirectUris = { "https://localhost:5160/signin-oidc" },

                // where to redirect to after logout
                PostLogoutRedirectUris = { "https://localhost:5160/signout-callback-oidc" },

                AllowOfflineAccess = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "shop",
                }
            }
            };
    }
}
